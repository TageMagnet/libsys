<?php

// Directory paths set up
$paths = [
    'app' => realpath(__DIR__),
    'public_html' => getcwd(),
    'view' => realpath(__DIR__  . DIRECTORY_SEPARATOR . 'view'),
    'lib' => realpath(__DIR__  . DIRECTORY_SEPARATOR . 'lib'),
    'typelib' => realpath(__DIR__  . DIRECTORY_SEPARATOR . 'typelib'),
    'controller' => realpath(__DIR__  . DIRECTORY_SEPARATOR . 'controller'),
    'vendor' => realpath(__DIR__) . DIRECTORY_SEPARATOR . '../vendor/',
    'directory' => realpath(__DIR__  . DIRECTORY_SEPARATOR . 'directory'),
    'ebooks' => realpath(__DIR__  . DIRECTORY_SEPARATOR . 'directory/ebooks'),
    'libsys' => realpath(__DIR__  . DIRECTORY_SEPARATOR . 'directory/libsys'),
];
defined('PATHS') or define('PATHS', $paths);


// Load Vendor AutoLoader
include_once(PATHS['vendor'] . "autoload.php");

// Environment file
$dotenv = Dotenv\Dotenv::createImmutable(__DIR__);
$dotenv->load();

// Autoloader for stuff
spl_autoload_register(function ($name) {

     // Controllers called by router
    if (strpos($name, 'Controller') !== false) {
        include_once(PATHS['controller'] . DIRECTORY_SEPARATOR . "$name.php");
        return;
    }

    // Models in /typelib
    $scanned_directory = array_diff(scandir(PATHS['typelib']), array('..', '.'));
    foreach ($scanned_directory as $key => $val) {
        if (basename($name) === $name) {
            include_once(PATHS['typelib'] . DIRECTORY_SEPARATOR . $val);
        }
    }
});
