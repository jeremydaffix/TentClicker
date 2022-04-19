<?php

	require_once 'config.php';

	//print_r($_POST);
	
	$pdo = ConnectDatabase();
	DispatchRequest();
	
	
	// ********************


	function ConnectDatabase()
	{
		global $config_sgbdr, $config_host, $config_db, $config_user, $config_password;
		
		try
		{
			$connection = "$config_sgbdr:host=$config_host;dbname=$config_db";
			
			//echo($connection);
			
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
		
	}
	

	function GenerateID()
	{
		$id = strtoupper(substr(uniqid(), -6));
		return $id;
	}

?>