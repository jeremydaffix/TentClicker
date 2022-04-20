<?php

	// *** script principal de création / récupération d'une sauvegarde de jeu ***

	require_once 'config.php'; // variables de configuration de l'accès à la bdd
	
	$pdo = ConnectDatabase(); // connexion
	DispatchRequest(); // gestion de la fonction de l'API à appeler selon le type de requête
	
	
	// ********************


	// fonction de connexion à la bdd selon la configuration
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

	// fonction appelant le bon code selon le verbe http utilisé,
	// dans la logique REST
	// game/ + POST -> création
	// game/:id (GET) -> récupération
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
				 
			default: // autre : retour d'une erreur http
				header("HTTP/1.0 405 Method Not Allowed");
				break;
		}
	}

	// fonction de création d'une sauvegarde de jeu
	function CreateGame()
	{	
		global $pdo;
	
		// vérification de l'existence et de la validité des paramètres passés
		if(isset($_POST["score"]) && $_POST["score"] != "" &&
		   isset($_POST["clickUpgradeLevel"]) && $_POST["clickUpgradeLevel"] != "" &&
		   isset($_POST["autoGatherUpgradeLevel"]) && $_POST["autoGatherUpgradeLevel"] != "")
		{
			$score = intval($_POST["score"]);
			$clickUpgradeLevel = intval($_POST["clickUpgradeLevel"]);
			$autoGatherUpgradeLevel = intval($_POST["autoGatherUpgradeLevel"]);
			
			//echo "$score  $clickUpgradeLevel  $autoGatherUpgradeLevel";
			
			$id = GenerateID(); // identifiant "aléatoire" sur 6 caractères
			
			$sql = "INSERT INTO game VALUES(:id, :score, :clickUpgradeLevel, :autoGatherUpgradeLevel)"; // code requête préparée

			try
			{
				$statement = $pdo->prepare($sql);

				// exécution de la requête préparée
				$statement->execute([
					':id' => $id,
					':score' => $score,
					':clickUpgradeLevel' => $clickUpgradeLevel,
					':autoGatherUpgradeLevel' => $autoGatherUpgradeLevel
				]);
				
				$statement = null;
				
				die($id); // on retourne au client l'identifiant de la sauvegarde créée
			}
			
			// erreur lors de l'exécution : on ne retourne rien au client
			catch(PDOException $e)
			{
				//$msg = 'PDO ERROR in ' . $e->getFile() . ' L.' . $e->getLine() . ' : ' . $e->getMessage();
				//die($msg);
				die();
			}
		}
		
		// erreur dans les paramètres : on ne retourne rien au client
		else
		{
			die();
		}
	}

	// fonction de récupération d'une sauvegarde de jeu
	function GetGame()
	{
		global $pdo;

		// vérification de l'existence et de la validité des paramètres passés
		if(isset($_GET["id"]) && $_GET["id"] != "" && strlen($_GET["id"]) == 6)
		{
			$id = strtoupper($_GET["id"]);
			
			//echo "GET GAME WITH ID $id";
			
			$sql = 'SELECT * FROM game WHERE id=:id'; // code requête préparée
			
			try
			{
				$statement = $pdo->prepare($sql);
				 
				// exécution de la requête préparée
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
			
			// erreur dans la requête : on retourne une erreur 404 (objet demandé non trouvé)
			catch(PDOException $e)
			{
				//$msg = 'PDO ERROR in ' . $e->getFile() . ' L.' . $e->getLine() . ' : ' . $e->getMessage();
				//die($msg);
				header("HTTP/1.0 404 Not Found");
				die();
			}
		}
		
		// erreur dans les paramètres : on retourne une erreur 404 (objet demandé non trouvé)
		else
		{
			header("HTTP/1.0 404 Not Found");
			die();
		}
	}
	

	// fonction de génération d'un identifiant "aléatoire"
	function GenerateID()
	{
		//return "AAA000"; // pour tester les erreurs sans identifiant unique
		$id = strtoupper(substr(uniqid(), -6));
		return $id;
	}

?>