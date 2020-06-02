<?php

// Retrieve complete url
$url = full_url( $_SERVER );

// Pick apart url
$parts = [
	'scheme'=>$scheme,
	'host'=>$host,
	'port' => $port,
	'path' => $path
	] = parse_url($url);

// Splits the '='
parse_str(parse_url($url, PHP_URL_QUERY), $output);
$parts['query'] = $output;
$parts['fragment'] = parse_url($url, PHP_URL_FRAGMENT);

// Routing based on path

if(preg_match("/^\/libsys/", $path))
{
	// /var/www/libsys
	(@include_once('../libsys/index.php')) or die("oops, nothing found");
}
else
{
	// 404 response
	header($_SERVER["SERVER_PROTOCOL"]." 404 Not Found", true, 404);
 	echo "404 - the path was not found, homeboy";
}



function url_origin( $s, $use_forwarded_host = false )
{
    $ssl      = ( ! empty( $s['HTTPS'] ) && $s['HTTPS'] == 'on' );
    $sp       = strtolower( $s['SERVER_PROTOCOL'] );
    $protocol = substr( $sp, 0, strpos( $sp, '/' ) ) . ( ( $ssl ) ? 's' : '' );
    $port     = $s['SERVER_PORT'];
    $port     = ( ( ! $ssl && $port=='80' ) || ( $ssl && $port=='443' ) ) ? '' : ':'.$port;
    $host     = ( $use_forwarded_host && isset( $s['HTTP_X_FORWARDED_HOST'] ) ) ? $s['HTTP_X_FORWARDED_HOST'] : ( isset( $s['HTTP_HOST'] ) ? $s['HTTP_HOST'] : null );
    $host     = isset( $host ) ? $host : $s['SERVER_NAME'] . $port;
    return $protocol . '://' . $host;
}

function full_url( $s, $use_forwarded_host = false )
{
    return url_origin( $s, $use_forwarded_host ) . $s['REQUEST_URI'];
}

/**
 * Join all path arguments
 * @param rest ...
 * @return void
 */
function joinPaths()
{
    return preg_replace('#/{2,}#', '/', implode('/', array_filter(func_get_args())));
}
