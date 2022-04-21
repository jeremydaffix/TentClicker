# Tent Clicker

## Mise en route
-  Projet Unity : Ouvrir le répertoire *Unity/* avec Unity 2019.4.32f1.
- Serveur web : Copier le répertoire *PHP/tentclicker/* dans le *www/* de wampserver (testé avec l'installation par défaut de wampserver). Ne pas changer le nom du répertoire.
- Base de données : importer *SQL/tentclicker.sql* dans une base de données MySQL nommée **tentclicker**, avec l'encodage par défaut (alternative : modifier *PHP/tentclicker/config.php* pour changer la base de données utilisée, ou l'utilisateur / mot de passe).

## Fonctionnalités implémentées
Les 3 parties ont été traitées, ainsi que l'ensemble des fonctionnalités demandées.

## Remarques sur le serveur PHP
Je me suis inspiré de l'architecture REST pour concevoir l'API. Celle-ci associe un format d'URI et l'utilisation des méthodes du protocole HTTP (POST, GET, PUT...) pour modifier et accéder à des ressources.
Dans le cadre de cet API simplifiée cela donne donc :
- http://localhost/tentclicker/game/ + Données passées par POST afin de créer une nouvelle sauvegarde.
- http://localhost/tentclicker/game/:id (redirigé en game?id=:id) pour accéder à une sauvegarde existante (retournée au format JSON).

Pour obtenir ces routes / formats d'URI, il a été nécessaire de faire de l'URL Rewriting avec un fichier *.htaccess* (module Apache activé par défaut dans wampserver).

L'API *PDO* intégrée à PHP a été utilisée pour disposer d'une abstraction plus grande du SGBDR, et surtout pouvoir utiliser les requêtes préparées qui ajoutent une plus grande sécurité (notamment contre les injections SQL, en découplant les données envoyées par l'utilisateur des requêtes).

## Remarques sur la base de données
Pour les tests, l'utilisateur par défaut (**root**, sans mot de passe) a été utilisé.
Deux tables ont été créées :
- **game**, qui contient l'identifiant unique et les données de base de la sauvegarde (nombre de ressources, niveaux des upgrades).
- **decoration**, représentant une décoration (type, numéro de ligne, numéro de colonne), et liée à un game par son identifiant.

Le moteur InnoDB, plus performant que MyISAM, a été sélectionné pour ces tables afin de pouvoir utiliser une contrainte de Foreign Key (garantissant l'intégrité de la BDD).

## Remarques sur le client Unity
- Le modèle (*Scripts/Model/*) est séparé de la logique de l'application ("Managers", sous forme de components/singleton). A noter qu'une version serializable, dont les noms de champs correspondent exactement aux noms de la base de données, a été mise en place pour pouvoir désérialiser directement le JSON retourné par le serveur en objets C# (avec *JsonUtility*).
- *UIManager* a la responsabilité de faire le lien avec les éléments d'UI Unity. A noter que la mise à jour de l'interface n'est pas effectuée dans une boucle *Update()*, mais seulement appelée par les contrôleurs lorsque les données sont changées.
- *GameManager* contient la logique générale du jeu .
- *SaveManager* contient les méthodes nécessaires à la sauvegarde / le chargement, notamment les appels à l'API web.
- *DecorationManager*  s'occupe de la gestion des décorations, de leur ajout dans la scène, du nettoyage de la grille lors d'un chargement, de la mise à jour du modèle...
- L'aspect de l'UI n'est pas très raffiné, mais utilise une palette de couleurs : https://coolors.co/palette/d9ed92-b5e48c-99d98c-76c893-52b69a-34a0a4-168aad-1a759f-1e6091-184e77