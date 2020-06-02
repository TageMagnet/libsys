<?php

use Medoo\Medoo;

class LibsysController extends Controller
{
    // Reset database connection to null, since we are using a foreign server
    protected $conn = null;

    /**
     * DELETE api.jkb.zone/file/:name
     * Delete file with the specified :name parameter
     *
     * @param [type] $req
     * @param [type] $res
     * @return void
     */
    public function DeleteFile($req, $res): void
    {
        $jsonResponse = [
            'success' => false,
            'message' => 'Unknown error'
        ];

        $files = array_diff(scandir(PATHS['libsys']), ['..', '.']);
        $name = urldecode($req->params['name']);
        $needle = null;

        foreach ($files as $key => $file) {
            // strpos to check if part of file matching || full string comparison
            if (strpos($file, $name) != false || $file == $name) {
                $needle = $key;
                break;
            }
        }

        if (!is_numeric($needle)) {
            http_response_code(405);
            $jsonResponse['success'] = false;
            $jsonResponse['message'] = "Could not find file with the name '$name'";
            // returns void and breaks function
            $res->json($jsonResponse);
        }

        // todo; actgual delete not yet implemented

        $ff = $files[$needle];
        $fileToDelete = joinPaths(PATHS['libsys'], $files[$needle]);
        http_response_code(200);
        $jsonResponse['success'] = true;
        $jsonResponse['message'] = "Found '$ff'";
    }

    /**
     * GET api.jkb.zone/file
     * List stored files
     *
     * @param [type] $req
     * @param [type] $res
     * @return void
     */
    public function List($req, $res): void
    {
        $files = array_diff(scandir(PATHS['libsys']), ['..', '.']);
        $arr = [];
        $debug = '';

        foreach ($files as $key => $file) {

            $fileParts = [];
            $partArr = explode('.', $file, 4);

            if (count($partArr) == 3) {
                foreach (['unix_timestamp', 'hashed_filename', 'file_ending'] as $k) {
                    $part = array_shift($partArr);
                    $fileParts[$k] = $part;
                }
            }

            if (count($partArr) == 4) {
                foreach (['unix_timestamp', 'hashed_filename', 'random_string', 'file_ending'] as $k) {
                    $part = array_shift($partArr);
                    $fileParts[$k] = $part;
                }
            }

            $arr[] = [
                'index' => $key,
                'name'  => $file,
                'size'  => filesize(joinPaths(PATHS['libsys'], $file)),
                'file_parts'  => $fileParts,
                'debug' => $debug
            ];
        }

        $res->json($arr);
    }

    /**
     * GET api.jkb.zone/file/:name
     * Download file
     *
     * @param [type] $req
     * @param [type] $res
     * @return void
     */
    public function GetFileByName($req, $res): void
    {
        $files = array_diff(scandir(PATHS['libsys']), ['..', '.']);
        $name = urldecode($req->params['name']);
        $needle = null;

        foreach ($files as $key => $file) {
            // strpos to check if part of file matching || full string comparison
            if (strpos($file, $name) != false || $file == $name) {
                $needle = $key;
                break;
            }
        }

        // A successfull search should return a numeric, so indirectly a null and false check
        if (!is_numeric($needle)) {
            http_response_code(404);
            $res->write("Could not find resource");
        }

        // Generate full path
        $downloadPath = joinPaths(PATHS['libsys'], $files[$needle]);

        // Probably redundant check to make sure file exist
        if (!file_exists($downloadPath)) {
            http_response_code(500);
            $res->write("Error finding file");
        }

        // Set up to make the users' web browser react appropriately and download the file
        header("X-Sendfile: $downloadPath");
        header("Content-Length: " . filesize($downloadPath));
        header("Content-type: application/octet-stream");
        header('Content-Disposition: attachment; filename="' . basename($downloadPath) . '"');
        readfile($downloadPath);
    }

    /**
     * POST api.jkb.zone/file
     * Places uploaded file in specified folder and generates a new filename for it
     *
     * @param [type] $req
     * @param [type] $res
     * @return json
     */
    public function UploadFile($req, $res): void
    {
        // Example of incoming file
        //$_FILES["file"]=>
        // array(5) {
        //   ["name"]=> string(13) "nativelog.txt"
        //   ["type"]=> string(10) "text/plain"
        //   ["tmp_name"]=>string(14) "/tmp/phpd21wQ5"
        //   ["error"]=>int(0)
        //   ["size"]=>int(926)
        //   
        // }

        // Object to be returned in json format
        $jsonResponse = [
            'success' => false,
            'message' => 'Unknown error',
            'location' => '/',
            'filesize' => 0,
            'temp'  => []
        ];

        try {

            // 10490000 bytes =~ 10 mebibyte
            $maxFileSize = 10490000;
            // Check for total uploadsize not exceding max
            $totalSize = 0;
            // Allowed extenstion types
            $extTypes = ['gif', 'jpg', 'jpe', 'jpeg', 'png', 'txt', 'log', 'pdf', 'epub'];

            // ? todo; some kind of proper auth validation here
            if (!isset($_POST, $_POST['upload_preset']))
                throw new Exception("Missing authentication fields");

            // ? matches secret
            if (!strcasecmp($_POST['upload_preset'], 'boa'))
                throw new Exception("Invalid authentication");

            // Check that all properties exist
            if (!isset($_FILES['file'], $_FILES['file']['name'], $_FILES['file']['type'], $_FILES['file']['tmp_name'], $_FILES['file']['error'], $_FILES['file']['size']))
                throw new Exception("Incorrect format");

            // Loop all incoming files
            foreach ($_FILES as $key => $val) {

                // Clean filename of unwanted characters
                $val['name'] = preg_replace("/[^A-Za-z0-9åäöÅÄÖ .\-_]/", '', $val['name']);

                // variables
                $newFile = null;
                $extName = strtolower(pathinfo($val['name'], PATHINFO_EXTENSION));
                $baseName = pathinfo($val['name'], PATHINFO_BASENAME);

                $jsonResponse['x'] = $extName;

                // Max filesize check
                if (($totalSize + $val['size']) > $maxFileSize)
                    throw new Exception("Max filesize reached");

                // Check file type
                if (!in_array($extName, $extTypes))
                    throw new Exception("Unallowed file type");

                // ? Uploaded file exist
                if (!file_exists($val['tmp_name']))
                    throw new Exception("No file found");

                // Mask the filename
                // UNIXTIME.HASHEDNAME.RANDOMSTRING.FILEENDING
                //$category = "XYZ";
                $hashedName = hash('sha1', $baseName);
                $unixTime = strval(time());
                $fileEnding = $extName;
                $randomString = random_str(7);
                $hashedFilename = sprintf('%d.%s.%s.%s', $unixTime, $hashedName, $randomString, $fileEnding);
                $newFile = joinPaths(PATHS['libsys'], $hashedFilename);

                // ? File was succesfully uploaded
                if (!move_uploaded_file($val['tmp_name'], $newFile))
                    throw new Exception("Upload failed");

                // Set the filename to response
                $jsonResponse['location'] = $hashedFilename;
                $jsonResponse['filesize'] = $val['size'];

                // increment total size
                $totalSize += $val['size'];
            }

            // End
            $jsonResponse['success'] = true;
            $jsonResponse['message'] = "Success, totalsize=$totalSize ";
            $res->json($jsonResponse);
        } catch (Exception $e) {
            $code = $e->getCode();
            // $e->getCode()) defaults to '0', but we rather want a 500
            $code = $code != '0' ? $code : 500;
            http_response_code($code);
            $jsonResponse['success'] = false;
            $jsonResponse['message'] = $e->getMessage();
            $res->json($jsonResponse);
        }
    }

    /**
     * GET api.jkb.zone/upload-form
     * HTML-page with a form for uploading stuff
     *
     * @param [type] $req
     * @param [type] $res
     * @return void
     */
    public function UploadForm($req, $res)
    {
        $f = joinPaths(PATHS['view'], 'upload-form.php');
        if (!file_exists($f))
            $res->write("Error");

        include($f);
    }

    /**
     * Undocumented function
     *
     * @param [type] $req
     * @param [type] $res
     * @return void
     */
    public function TestConnection($req, $res)
    {
        echo "Not working atm";
    }

    /**
     * Check if unix timestamp is valid
     *
     * @param [type] $string
     * @return boolean
     */
    private function isTimestamp($string): bool
    {
        try {
            new DateTime('@' . $string);
        } catch (Exception $e) {
            return false;
        }
        return true;
    }
}
