<?php
/**
 * Routing based on URL path
 */
(@include_once(PATHS['lib'] . DIRECTORY_SEPARATOR . 'jRoute.php')) or die("oops, no route");

$route = new jRoute;

// MiddleWare
$route::use(function ($req) {
    // Starting timer for speed performance
    $req->start = microtime(true);
});

$route::GET("/upload-form","LibsysController@UploadForm",[]);
$route::GET("/file","LibsysController@List",[]);
$route::GET("/file/:name","LibsysController@GetFileByName",[]);
$route::POST("/file","LibsysController@UploadFile",[]);
$route::DELETE("/file/:name","LibsysController@DeleteFile",[]);
$route::GET("/test","LibsysController@TestConnection",[]);


// Fallback catcher for all GET-requests
$route::GET("/.*$/", function ($req, $res) {

    $dirs = array_filter(explode('/', $req->path), function ($a) {
        return $a !== null && $a !== '' ? true : false;
    });

    $tempstr = "";
    foreach ($dirs as $key => $val) {
        $tempstr .= "<b class=\"rainbow\">/$val</b>";
    }

    // $directories is used in the view
    $directories = $tempstr;
    //
    $query = "?";
    foreach($req->query as $key => $val)
    {
        $query .= "$key=$val&";
    }
    $query = substr($query, 0, -1);
    (@include_once(PATHS['app'] . DIRECTORY_SEPARATOR . 'view/home.php')) or die("505, error");

}, []);

// Initialize the router
$route::route();

