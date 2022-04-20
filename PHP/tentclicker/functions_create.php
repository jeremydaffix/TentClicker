<?php

	// *** fonctions pour la création d'une sauvegarde ***
	

	// fonction de création d'une sauvegarde de jeu
	function CreateGame()
	{	
		// vérification de l'existence et de la validité des paramètres passés
		if(isset($_POST["score"]) && $_POST["score"] != "" &&
		   isset($_POST["clickUpgradeLevel"]) && $_POST["clickUpgradeLevel"] != "" &&
		   isset($_POST["autoGatherUpgradeLevel"]) && $_POST["autoGatherUpgradeLevel"] != "" &&
		   isset($_POST["decorations"]) && $_POST["decorations"] != "")
		{
			// parsage des paramètres
			$score = intval($_POST["score"]);
			$clickUpgradeLevel = intval($_POST["clickUpgradeLevel"]);
			$autoGatherUpgradeLevel = intval($_POST["autoGatherUpgradeLevel"]);
			$decorationsArr = json_decode($_POST["decorations"], true);
			
			$id = InsertGame($score, $clickUpgradeLevel, $autoGatherUpgradeLevel);
			InsertDecorations($id, $decorationsArr);
		}
		
		// erreur dans les paramètres : on ne retourne rien au client
		else
		{
			die();
		}
		
		die($id); // on retourne l'identifiant au client
	}
	
	
	function InsertGame($score, $clickUpgradeLevel, $autoGatherUpgradeLevel)
	{
		// *** 1ère partie : création d'une entrée dans la table game
		
		global $pdo;
			
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
		}
		
		// erreur lors de l'exécution : on ne retourne rien au client
		catch(PDOException $e)
		{
			//$msg = 'PDO ERROR in ' . $e->getFile() . ' L.' . $e->getLine() . ' : ' . $e->getMessage();
			//die($msg);
			die();
		}
		
		return $id;
	}
	
	function InsertDecorations($id, $decorationsArr)
	{
		// *** 2ème  partie : création des entrées dans la table decorations

		global $pdo;

		$sqlDeco = "INSERT INTO decoration VALUES(:id_game, :type, :row, :col)"; // code requête préparée

		try
		{		
			$statementDeco = $pdo->prepare($sqlDeco);

			foreach($decorationsArr as $decoration)
			{
				// exécution de la requête préparée
				$statementDeco->execute([
					':id_game' => $id,
					':type' => $decoration['type'],
					':row' => $decoration['row'],
					':col' => $decoration['col']
				]);
			}
				
			$statementDeco = null;
		}
			
		// erreur lors de l'exécution : on ne retourne rien au client
		catch(PDOException $e)
		{
			//$msg = 'PDO ERROR in ' . $e->getFile() . ' L.' . $e->getLine() . ' : ' . $e->getMessage();
			//die($msg);
			die();
		}
	}

?>