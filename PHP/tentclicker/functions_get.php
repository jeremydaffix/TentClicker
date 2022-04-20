<?php

	// *** fonctions pour la récupération d'une sauvegarde ***


	// fonction de récupération d'une sauvegarde de jeu
	function GetGame()
	{
		// vérification de l'existence et de la validité des paramètres passés
		if(isset($_GET["id"]) && $_GET["id"] != "" && strlen($_GET["id"]) == 6)
		{
			$id = strtoupper($_GET["id"]);
						
			$game = SelectGame($id);
			$decos = SelectDecorations($id);
			
			$game["decorations"] = $decos;
			
			header('Content-Type: application/json');
			die(json_encode($game));
		}
		
		// erreur dans les paramètres : on retourne une erreur 404 (objet demandé non trouvé)
		else
		{
			header("HTTP/1.0 404 Not Found");
			die();
		}
	}
	
	
	function SelectGame($id)
	{
		// *** 1ère partie : récupération de l'entrée dans la table game

		global $pdo;

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
				
			//header('Content-Type: application/json');
			//die(json_encode($arrAll[0]));

			return $arrAll[0];
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
	
	function SelectDecorations($id)
	{
		// *** 2ème  partie : récupération des entrées dans la table decorations

		global $pdo;
		
		$sql = 'SELECT * FROM decoration WHERE id_game=:id'; // code requête préparée
			
		try
		{
			$statement = $pdo->prepare($sql);
				 
			// exécution de la requête préparée
			$statement->execute([
				':id' => $id
			]);
				 
			$arrAll = $statement->fetchAll(PDO::FETCH_ASSOC);
				 
			$statement->closeCursor();
			$statement = NULL;
			
			return $arrAll;
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
	
?>