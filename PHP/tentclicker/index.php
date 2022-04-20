<?php

	require_once 'config.php';
	
	$pdo = ConnectDatabase();
	DispatchRequest();
	
	
	// ********************


	function ConnectDatabase()
	{
		global $config_sgbdr, $config_host, $config_db, $config_user, $config_password;
		
		try
		{
			$connection = "$config_sgbdr:host=$config_host;dbname=$config_db";
	
			//$arrExtraParam= array(PDO::MYSQL_ATTR_INIT_COMMAND => "SET NAMES utf8");
			$pdo = new PDO($connection, $config_user, $config_password/*, $arrExtraParam*/);
			$pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

			return $pdo;
		}

		catch(PDOException $e)
		{
			//$msg = 'PDO ERROR in ' . $e->getFile() . ' L.' . $e->getLine() . ' : ' . $e->getMessage();
			//die($msg);
			die();
		}
	}

	function DispatchRequest()
	{
		$request_method = $_SERVER["REQUEST_METHOD"];
		
		switch($request_method)
		{
			case 'GET':
				GetGame();
				break;
				  
			case 'POST':
				CreateGame();
				break;
				 
			default:
				header("HTTP/1.0 405 Method Not Allowed");
				break;
		}
	}

	function CreateGame()
	{	
		global $pdo;
	
		if(isset($_POST["score"]) && $_POST["score"] != "" &&
		   isset($_POST["clickUpgradeLevel"]) && $_POST["clickUpgradeLevel"] != "" &&
		   isset($_POST["autoGatherUpgradeLevel"]) && $_POST["autoGatherUpgradeLevel"] != "")
		{
			$score = intval($_POST["score"]);
			$clickUpgradeLevel = intval($_POST["clickUpgradeLevel"]);
			$autoGatherUpgradeLevel = intval($_POST["autoGatherUpgradeLevel"]);
			
			//echo "$score  $clickUpgradeLevel  $autoGatherUpgradeLevel";
			
			$id = GenerateID();
			
			$sql = "INSERT INTO game VALUES(:id, :score, :clickUpgradeLevel, :autoGatherUpgradeLevel)";

			try
			{
				$statement = $pdo->prepare($sql);

				$statement->execute([
					':id' => $id,
					':score' => $score,
					':clickUpgradeLevel' => $clickUpgradeLevel,
					':autoGatherUpgradeLevel' => $autoGatherUpgradeLevel
				]);
				
				$statement = null;
				
				die($id);
			}
			
			catch(PDOException $e)
			{
				//$msg = 'PDO ERROR in ' . $e->getFile() . ' L.' . $e->getLine() . ' : ' . $e->getMessage();
				//die($msg);
				die();
			}
		}
		
		else
		{
			die();
		}
	}

	function GetGame()
	{
		global $pdo;

		if(isset($_GET["id"]) && $_GET["id"] != "" && strlen($_GET["id"]) == 6)
		{
			$id = strtoupper($_GET["id"]);
			
			//echo "GET GAME WITH ID $id";
			
			$sql = 'SELECT * FROM game WHERE id=:id';
			
			try
			{
				$statement = $pdo->prepare($sql);
				 
				$statement->execute([
					':id' => $id
				]);
				 
				$arrAll = $statement->fetchAll(PDO::FETCH_ASSOC);
				
				if($statement->rowCount() != 1)
				{
					header("HTTP/1.0 404 Not Found");
					die();
				}
				 
				$statement->closeCursor();
				$statement = NULL;
				
				header('Content-Type: application/json');
				die(json_encode($arrAll[0]));
			}
			
			catch(PDOException $e)
			{
				//$msg = 'PDO ERROR in ' . $e->getFile() . ' L.' . $e->getLine() . ' : ' . $e->getMessage();
				//die($msg);
				header("HTTP/1.0 404 Not Found");
				die();
			}
		}
		
		else
		{
			header("HTTP/1.0 404 Not Found");
			die();
		}
	}
	

	function GenerateID()
	{
		//return "AAA000"; // pour tester les erreurs sans identifiant unique
		$id = strtoupper(substr(uniqid(), -6));
		return $id;
	}

?>