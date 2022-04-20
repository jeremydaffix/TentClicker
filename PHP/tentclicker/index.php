<?php

	// *** script principal de création / récupération d'une sauvegarde de jeu ***

	require_once 'config.php'; // variables de configuration de l'accès à la bdd
	require_once 'functions_create.php'; // fonctions pour la création d'une sauvegarde
	require_once 'functions_get.php'; // fonctions pour la récupération d'une sauvegarde
	
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

	// fonction de génération d'un identifiant "aléatoire"
	function GenerateID()
	{
		//return "AAA000"; // pour tester les erreurs sans identifiant unique
		$id = strtoupper(substr(uniqid(), -6));
		return $id;
	}

?>