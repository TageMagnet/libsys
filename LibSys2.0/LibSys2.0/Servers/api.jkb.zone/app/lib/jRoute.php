<?php
/**
 * 
 * 	─=≡Σ((( つ◕ل͜◕)つ
 * -t	jRoute - just Route
 * -d	Simple php router
 * -a 	jkb
 * -s	https://gitlab.com/sketcher/justroute
 * 
 */
class jRoute
{
	static $paths = array();
	static $groups = [];
	static $errorcodeGroups = [];
	static $codes = array(
		'204'	=>	'No content',
		'403'	=>	'Unauthorized',
		'404'	=>	'Not found',
		'405'	=>	'Exists. But wrong request method type',
		'500'	=>	'Interval server error',
		'501'	=>	'Method not implemented'
	);
	static $req;
	static $res;
	static $middleware = [];
	static $options = [
		//True : e.g. url '127.0.0.1/folder/website' =(turns into)=> '/'
		//False: no change
		'PATH_RESET'		=> true,
		//Http request type not in this list throws 501 error
		'ALLOWED_METHODS'	=> ['GET', 'POST', 'UPDATE', 'DELETE', 'PUT', ''],
		//todo; Settings for Error display. 
		'HARD_ERROR'		=> false,
		//todo; Tool for dry-running all server paths, without invoking any functions. 
		'DRY_RUN'			=>	false
	];
	static $error = false;

	/**
	 * Main function. Parses through all set routes on request
	 *
	 * @param array $groups
	 * @param boolean $next
	 * @return void
	 */
	public static function route($groups = [], $next = false)
	{
		/************************************************************

		 ***********************************************************/
		try{
			
			// Supplied request's authorized groups, [] = all allowed
			self::$groups = $groups;

			// Creating response object
			self::setResponseObject();
			// Middleware before request is handled
			self::retrieveRequests();
			self::applyMiddleware(self::$req);

			// Hard error if non-listed request method type
			self::allowedMethods(self::$options['ALLOWED_METHODS'], self::$req->method);

			// Scoping inside array_filter()
			$path 	= self::$req->path;
			$method = self::$req->method;

			// Clear  the $path of possible trailing backslash
			$path = (strrpos($path, '/') !== false && strrpos($path, '/') === (strlen($path) - 1) &&
			strlen($path) > 1 ? substr($path, 0, -1) : $path);

			// Scoping inside array_filter()
			$request = self::$req;// single

			//	$var | error code
			//	=> $pathKey 	|| 404
			//	=> $methodKey 	|| 405
			//	=> $authKey 	|| 403
			$pathKey 	= [false, 404];
			$methodKey 	= [false, 405];
			$authKey 	= [false, 403];

			/**
			 *	A. Collects all Path Objects with correct URI-path or correct :PARAM-count
			 *	Else 404 response code. And with custom page if is set 
			 */
			self::$paths = array_map(function($c) use($request){
				// ? has ':', copy the input-request path
				if(strpos($c['path'], ':')){
					$c['path'] = $request->path;
				}
				return $c;
			}, array_filter(self::$paths, function(&$a) use($request){

					// ? regex-string as path. Execute it against request-path and see if match, else return string with URL-invalid characters
					if($a['isRegex']){
						$a['path'] = preg_match($a['path'], $request->path) ? $request->path : "#<>#%>##<>#%>##<>#%>##";
					}
					// Incorrect path or Incorrect parameter count filtered out
					//$a['paramCount'] == $request->queryParameterCount
					return ( $a['path'] == $request->path ? true :  self::areParamsMatching($a['param']) && $a['param'] ? true : false)  !== false;
				})
			);

			if (empty(self::$paths)) {
				$pathKey[0] = true;
			}

			/**
			 *	B. Filter for correct Request Method
			 *	Else 405
			 */
			self::$paths = array_filter(self::$paths, function($a) use($request){
				return( $a['method'] == $request->method);
			});

			if (empty(self::$paths) && !$pathKey[0]) {
				$methodKey[0] = true;
			}

			/**
			 *	C. Compares against set Group Authority
			 *	If fail; 403
			 */	
			self::$paths = array_filter(self::$paths, function($a) use($groups){
				if(self::belongsToAccessGroup($groups, $a['group'])){
					return $a;
				}
			});

			if (empty(self::$paths) && !$pathKey[0] && !$methodKey[0]) {
				$authKey[0] = true;
	   		}
	
			/**
			 * D. Runs set or default response corresponding to result
			 * Else one of previous mentioned error response codes
			 */
			if(!empty(self::$paths)){
				// Only use the first element, clear rest of Empty
				self::$paths = reset(self::$paths);

				//...
				self::setParamValuesAsKeys();
				try{
					// If Controller@Method is supplied instead of a callback
					if(is_string(self::$paths['function'])){
						list($className, $methodName) = explode("@", self::$paths['function']);
						if(class_exists($className) && method_exists($className, $methodName)){
    						$_ = new $className();
    						$_->$methodName(self::$req, self::$res);
						}
						else{
							throw new Exception('Class or method non-existant');
						}
					}
					else{
						call_user_func(self::$paths['function'], self::$req, self::$res);
					}
					
				}
				catch(Exception $e){
					echo $e->getMessage() . PHP_EOL;
					self::runErrorCode(500);
				}
			}
			else if($pathKey[0] || $methodKey[0] || $authKey[0]){
				$keyArr = [$pathKey, $methodKey, $authKey];
				$errorCode = array_filter($keyArr, function($v,$k){
					if($v[0]){
						self::runErrorCode($v[1]);
						die();
					}
				}, ARRAY_FILTER_USE_BOTH);	
			}
			else{
				self::runErrorCode(204);
			}
		}
		/**
		 * todo;
		 * - Set up some working error handling
		 */
		catch(Exception $e){
			echo "Error occured : ";
			self::runErrorCode(500);
		}
		finally{
			die();//safety death x-(
		}
	}

	/**
	 * Entry point for all method calls
	 * @param String $name
	 * @return Array $args
	 * @return Void
	 */
    public static function __callStatic(string $methodName, array $args)
    {
		self::$paths[] = array(
			'path'		=>	$args[0],
			'method'	=> 	strtoupper($methodName),
			'function'	=>	$args[1],
			'group'		=>  (isset($args[2]) ? $args[2]: []),
			'param'		=>	(strpos($args[0], ':') ? explode('/', trim($args[0], '/'))  : false),
			'paramCount'=> 	self::countParameters($args[0]),
			'isRegex'	=> 	self::isStringRegex($args[0])
		);
	}
	
	/**
	 * Supplies user defined functon
	 *
	 * @return Array ...
	 * @return Void
	 */
	public static function use()
	{
		$args = func_get_args();
		foreach($args as $key => $val){
			if(is_callable($val)){
				self::$middleware[] = $val;
			}
		}
	}

	/**
	 * Adjust default options
	 *
	 * @return Array
	 * @return Void
	 */
	public static function setOptions($default = []){
		foreach($default as $key => $val){
			if(array_key_exists($key, self::$options)){
				//Overrides current setting
				$options[$key] = $val;
			}
		}
	}
	
	public static function setCodeResponse($num, $func){
		self::$errorcodeGroups[(int)$num] = $func;
	}
	/**
	 * Set options for allowed request method
	 * todo; allowed methods that extends into a soft error control
	 * @param Array $methods
	 * @param String $reqMethod
	 * @return Void
	 */
	private static function allowedMethods($methods = ['GET', 'POST', 'UPDATE', 'DELETE', 'PUT', 'OPTIONS'], $reqMethod){
		$headersRecieved = headers_sent();
		$headers = headers_list();
		$r = array_search($reqMethod, $methods, TRUE);
		if($r === false){
			self::runErrorCode(501);
		}
	}

	/**
	 * Authorization validator
	 * @return Boolean
	 */
	private static function belongsToAccessGroup($in, $rq){
		$inArrIsEmpty = array_filter($in);
    	$rqArrIsEmpty = array_filter($rq);
    	return (empty($rqArrIsEmpty) ? true : !empty(array_intersect($rq, $in)) ? true : false);
	}

	/**
	 * Clear the ':'-character
	 *
	 * @return void
	 */
	private static function setParamValuesAsKeys(){
		if(self::$paths['param']){
			array_walk(self::$paths['param'], function(&$a){
				$a = str_replace(':', '', $a);
			});
			self::$req->params = array_combine(self::$paths['param'], self::$req->params);
		}
		else{
			self::$req->params = [];
		}
	}

	/**
	 * Returns complete url string
	 *
	 * @return String
	 */
	private static function getParsedUrl(){
		$x = parse_url((isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on' ? "https" : "http") . "://$_SERVER[HTTP_HOST]$_SERVER[REQUEST_URI]");
		return $x;
	}

	/**
	 * Echoes jRoute::setCode($code) if it's set, else it echoes the 3-number response code
	 * 
	 * todo; requires testing
	 *
	 * @param Integer $code
	 * @return Void
	 */
	private static function runErrorCode($code = 204){
		try{
			if(!preg_match('/^[0-9]{3}$/', $code)){
				throw new Exception ("Invalid error code '$code' supplied, need to be three numbers");
			}
			http_response_code(intval($code, 10));

			if(array_key_exists($code, self::$errorcodeGroups)){
				call_user_func(self::$errorcodeGroups[$code], self::$req, self::$res);
			}
			else{
				echo((string)$code);
			}
		}
		catch(Exception $e){
			http_response_code(500);
			echo "505 - Internal error";
			////////////////////////////////call_user_func(self::$codes['500']);
		}
	}

	/**
	 * Calls whatever middleware before route()
	 *
	 * @return Void
	 */
	private static function applyMiddleware(&$req){
		$callbacks = self::$middleware;
		foreach($callbacks as $key => $val){
			call_user_func($val, self::$req);
		}
	}

	/**
	 * Request Object
	 *
	 * @return Void
	 */
	private static function retrieveRequests(){
		self::$req = new stdClass;
		$url = self::getParsedUrl();
		foreach($url as $key => $prop){
			self::$req->{$key} = $prop;
		}
		self::$req->headers = [];
		
		// Polyfill for nginx. This was annoying to debug.
		// From stackoverflow/13224615
		if (!function_exists('getallheaders')){
			function getallheaders(){
				if (!is_array($_SERVER)) {
					return array();
				}
		
				$headers = array();
				foreach ($_SERVER as $name => $value) {
					if (substr($name, 0, 5) == 'HTTP_') {
						$headers[str_replace(' ', '-', ucwords(strtolower(str_replace('_', ' ', substr($name, 5)))))] = $value;
					}
				}
				return $headers;
			}
		}

		foreach(getallheaders() as $name => $value){
			self::$req->headers[$name] = $value;
		}

		self::$req->query       	= $_REQUEST;
		self::$req->uriParameterCount = count($_REQUEST);
		self::$req->method       	= $_SERVER['REQUEST_METHOD'];
		self::$req->body         	= file_get_contents("php://input");
		self::$req->cookies      	= $_COOKIE;
		if(self::$options['PATH_RESET']){
			self::$req->path      		= self::requestPath();
		}
		else{
			self::$req->path      		= $url;//Temporary, todo; change this
		}
	}

	/**
	 * Helper for :Params in URI
	 *
	 * @return Void
	 */
	private static function trimParamsFromUrl($parts){
		foreach($parts as $val){
			self::$req->params[] = strtok($val, '?');
		}
	}

	/**
	 * Return the number of parameters by '/'-slash for sorting
	 *
	 * @param String $pathStr
	 * @return void
	 */
	private static function countParameters($pathStr){
		return substr_count($pathStr, '/');
	}

	/**
	 * self explanatory placeholder
	 *
	 * @return Void
	 */
	private static function doNothing() : void {}

	/**
	 * todo; some kind of logger and soft error control
	 *
	 * @param [type] $args
	 * @return Void
	 */
	private static function _log($args){}

	/**
	 * 
	 */
	private static function areParamsMatching($arr) : bool{
		$bool = true;
		if(!is_array($arr)){
			return false;
		}
		foreach($arr as $key => $val){

			if(!isset(self::$req->params[$key])){
				$bool = false;
				break;
			}
			if( $val !== self::$req->params[$key] && substr($val,0,1) !== ':'){
				$bool = false;
				break;
			}
		}
		return $bool;
	}
	/**
	 * 
	 */
	private static function isStringRegex($s) : bool{
		return preg_match("/^\/.*\/[gmixsXuJ]{0,3}$/", $s);
	}
	/**
	 * Undocumented function
	 *
	 * @return Void
	 */
	private static function requestPath(){
		$request_uri = explode('/', trim($_SERVER['REQUEST_URI'], '/'));
		$script_name = explode('/', trim($_SERVER['SCRIPT_NAME'], '/'));
		$parts = array_values ( array_diff_assoc($request_uri, $script_name) );

		self::$req->queryParameterCount = count($parts);
		self::trimParamsFromUrl($parts);

		if (empty($parts)){
			return '/';
		}
		$path = implode('/', $parts);
		if (($position = strpos($path, '?')) !== FALSE){
			$path = substr($path, 0, $position);
		}
		return "/$path";
	}

	/**
	 * Response actions. Always final
	 *
	 * @return void
	 */
	private static function setResponseObject(){
		$response = new class {

			public function json($arr) { 
				// if json, echo out and die
				header('Content-Type: application/json; charset=utf-8');
				echo json_encode($arr, true);
				die();
			}

			public function image($image, $optionalType = 'png') {
				try{
					if(is_resource($image)){
						// header("Content-type: image/$optionalType");
						// imagepng($image);
						// destroyImage($image);
						throw new \Exception('Resource type not supported');
					}
					else if(is_string($image)){
						$ext = pathinfo($image, PATHINFO_EXTENSION);
						$isSupported = preg_match('/[\.]?(gif|jpg|jpeg|tiff|png|bmp|exif|raw)$/i', $ext);
						$imagecreatefrom = "imagecreatefrom$ext";

						if(!$isSupported){
							throw new \Exception('Image filetype not supported');
						}
						
						if(function_exists($imagecreatefrom)){
							header("Content-type: image/$ext");
							$im = call_user_func($imagecreatefrom, $image);
							call_user_func("image$ext", $im);
							imagedestroy($im);
						}

						else{
							throw new \Exception('Error occured on image creation');
						}
					}
					else{
						throw new \Exception('Error on supplied image');
					}
				}
				catch(Exception $e){

				}
				finally{
					die();
				}
			}

			public function write($str = ''){
				header('content-type: text/html; charset=utf-8');
				echo $str;
				die();
			}
			
		};
		self::$res = $response;
	}
}
