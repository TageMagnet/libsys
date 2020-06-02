<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <title>&#8452;&#10003;</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <styles></styles>
    <script type="text/javascript"></script>
</head>
<?php
/*
	-a jakob andersson
	-d school project
	
	$root_url = tjackobacco.com:23080/libsys
*/
// Include vendor autloader
require_once(joinPaths(dirname(__dir__),'libsys','vendor','autoload.php'));
use Medoo\Medoo;

try{
	
// Initiate database connection
$conn = new Medoo([
            'charset' => 'utf8mb4',
            'collation' => 'utf8mb4_general_ci',
            'database_type' => 'mysql',
            'database_name' => 'libsys',
            'server' => 'localhost',
            'username' => 'guest',
            'password' => 'hunter12'
]);

// Something happened to earlier files or server if this check fails
if (!isset($parts))
	Response("500 - error, missing variable", 500);

// $root_url?key=1234567980
if (!isset($parts['query']['key']))
	Response("400 - user error, missing key query", 400);
	
// Query based on query key
$arr = $conn->query(
    "SELECT * FROM <registrations> R JOIN <members> M WHERE M.member_id = R.ref_member_id AND R.link = :link;",
    ['link' => $parts['query']['key']]
)->fetchAll();

// Error check empty database return value
if (empty($arr))
	Response("invalid key", 400);

if (!is_array($arr))
	Response("data missing", 400);

// Deconstruct
['link' => $link, 
'createdAt' => $createdAt, 
'expiresAt' => $expiresAt, 
'is_active' => $isActive,
'member_id'	=> $memberId
] = $arr[0];

// Convert to unixtimestamp(seconds passed since 1 Jan 1970)
$now = strtotime(date('Y-m-d H:i:s'));
$then = strtotime($expiresAt);

// Check if more than 0, so already activated
if ($isActive > 0)
	Response("406 - You are already activated.", 406);

// Unixtimestamp is a higher number, so timeout occures if more than 0
if ($now - $then > 0)
	Response("406 - Your token has timed out.", 406);

// Update the affected row `Ã¬s_active` WHERE member_id matches
$ir = $conn->update("members",
[
	'is_active'=>1
],
[
	'member_id'=>$memberId
]);

Response("200 - You are activated", 200);

}
catch(Exception $e)
{
	Response("500 - Server error occured, exception was not handled.", 500);
}


function Response($responseMessage, $responseCode = 200)
{
	http_response_code($responseCode);
	echo $responseMessage;
	exit($responseCode);
}
?>
</html>
