<?php

/**
 * Check php7.x functionailty
 */
class phpSevenTest
{
    public function hello(): string
    {
        return 'Current PHP version : ' . phpversion();
    }
    public static function debug() : string {
        return realpath(__DIR__);
    }
}

/**
 * Parse whatever into useful formats
 */
class Parser
{
    /**
     * Retrieves the fully parsed URL from a web request
     * [protocol]://[authority][subdomain.][domain][.topdomain][:port]/path[?query][#fragment]
     * authority = [userinfo@]host[:port]~
     *
     * @return void
     */
    public static function GetFullURL(): string
    {
        // src:https://snipplr.com/view/40440/full-url-without-variables

        // ? https-protocol
        $isHttps = empty($_SERVER["HTTPS"]) ? '' : ($_SERVER["HTTPS"] == "on") ? "s" : "";

        // Retrieve actual protocol
        $protocol = substr(strtolower($_SERVER["SERVER_PROTOCOL"]), 0, strpos(strtolower($_SERVER["SERVER_PROTOCOL"]), "/")) . $isHttps;

        // Retrieve port
        $port = ($_SERVER["SERVER_PORT"] == "80") ? "" : (":" . $_SERVER["SERVER_PORT"]);

        // Combined
        $address = $protocol . "://" . $_SERVER['SERVER_NAME'] . $port . $_SERVER['REQUEST_URI'];
        $parseUrl = parse_url(trim($address));

        return $parseUrl['scheme'] . '://' . $parseUrl['host'] . $port . $parseUrl['path'];
    }

    public static function GetSubDirectories($uri): array
    {
        return explode('/', trim(parse_url($uri, PHP_URL_PATH), '/'));
    }
}

/**
 * Undocumented function
 *
 * @param [type] $name
 * @return void
 */
function GetJsonFile($name)
{
    $r = null;
    $scanned_directory = array_values(array_diff(scandir(PATHS['view']), array('..', '.')));
    foreach ($scanned_directory as $key => $val) {
       
        if ($val === $name . '.' . pathinfo($val, PATHINFO_EXTENSION)) {
            
            $r = file_get_contents(PATHS['view'] . DIRECTORY_SEPARATOR . $val);
            
            break;
        }
    }

    if ($r === false) {
        return false;
    }

    $json = json_decode($r, true);
    if ($json === null) {
        return false;
    }
    return $json;
}

function PrettyPrint($data)
{
    highlight_string("<?php\n " . var_export($data, true) . "?>");
    echo '<script>document.getElementsByTagName("code")[0].getElementsByTagName("span")[1].remove() ;document.getElementsByTagName("code")[0].getElementsByTagName("span")[document.getElementsByTagName("code")[0].getElementsByTagName("span").length - 1].remove() ; </script>';
}

/**
 * Join all path arguments
 * @param rest ...
 * @return void
 */
function joinPaths()
{
    $paths = array_filter(func_get_args());
    return preg_replace('#/{2,}#', '/', implode('/', $paths));
}

/**
 * Generate a random string, using a cryptographically secure 
 * pseudorandom number generator (random_int)
 *
 * This function uses type hints now (PHP 7+ only), but it was originally
 * written for PHP 5 as well.
 * 
 * For PHP 7, random_int is a PHP core function
 * For PHP 5.x, depends on https://github.com/paragonie/random_compat
 * 
 * @param int $length      How many characters do we want?
 * @param string $keyspace A string of all possible characters
 *                         to select from
 * @return string
 */
function random_str( int $length = 64, string $keyspace = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'): string {
    if ($length < 1) {
        throw new \RangeException("Length must be a positive integer");
    }
    $pieces = [];
    $max = mb_strlen($keyspace, '8bit') - 1;
    for ($i = 0; $i < $length; ++$i) {
        $pieces []= $keyspace[random_int(0, $max)];
    }
    return implode('', $pieces);
}