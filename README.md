# DeathCycle

Hello Max ! 

Je ne suis pas particulièrement fier de ce début de projet... j'ai hâte d'avoir tes retours :)

Comme il n'y a aucune instructions, aucun menu, aucun endgame... juste des mécaniques, voici ce qu'il faut savoir :

- C'est un jeu sencé être en multijoueur (au moins local)
- Contrôles
AD pour bouger à gauche/droite
ESPACE pour sauter
E pour taper
F pour capacité spéciale (nécessite du mana)
R pour retour (pendant la phase de séléction des champions)

- But : "capture the flag", le ramener à la base (plateforme en bleu)
- Particularité : time-loop. Chaque boucle est sencée répeter les actions des champions enregistrées lors des boucles précédentes, ce qui amène un côté stratégique intéressant, et un joyeux bordel au bout de la 5ème loop que compose une manche normale
- Gameplay : Pendant la première phase, séléctionner un spawnPoint et un champion avec A/D et E pour valider. Pendant la phase d'enregistrement: choper du mana, le drapeau, taper les adversaires.

## Mes impressions/retours:

- J'ai un problème fondamental : l'enregistrement/playback des actions des champions est foireuse.
J'essaie d'enregistrer les inputs du joueurs à chaque frame et de les restituer lors du playback. Mais forcément ça pose problème, dû a la variation du framerate.
J'ai tourné le problème dans tous les sens, je n'ai pas trouvé d'alternative qui fonctionne bien. Tout l'intérêt du jeu réside dans le playback des actions EXACTES des champions, donc tant que ce problème n'est pas résolu, il n'y a pas d'intérêt à continuer de bosser dessus..
J'espère que tu auras des idées :)

- Je suis pas sûr le fait que l'animation manager lise les animations pour set les bools d'autres scripts soit une bonne idée.
  
- Je suis pas sûr qu'avoir un animation manager par champion soit une bonne idée... un changement à effectuer sur un champion et hop, à faire dans chaque script. Tu auras peut-être des solutions :)

- Pour la gestion des tags et layers de colliders en type string, je sais que c'est pas ouf mais je vois pas trop comment faire autrement.
  
- Je trouve mon système de sélection de personnages assez dégeu à vrai dire.
  
- Je pense que j'ai assez bien géré les interfaces IChampionAttack et IChampionSpecial pour l'intégration de nouvelles manières d'attaquer (range, hybrid) et d'attaques spéciales

- Au niveau des scènes. "SandBox" est bizarre mais sert à tester. Je m'entraînait a mettre en place un tilemap donc c'est un peu dégeu. Tu verras dans "Arena" que j'ai essayé de créer une map, et j'ai mieux géré la tilemap. Mais je comprends pas pourquoi les colliders du tilemap font foirer mes animations et donc tout le contrôle des personnages.


Pour finir, j'ai quasiment tout écrit sans tuto (sauf pour le mouvement pour intégrer les forces), donc j'ai sûrement raté des moyens de faire des trucs plus proprement (spécialement pour les fonctions HandleChampionSelection() et HandleSpawnPointSelection() du script objectPool).


## Futures implémentations (si on trouve une solution pour la loop) :

- Capacité à sauter à travers les colliders marqués en tant que plateformes (en montant)
- Local multiplayer
- Une vraie map d'arène
- 3 champions supplémentaires (dont des ranged attacks)
- Wall jump pour certains persos
- Actions "Defend" et "roll"
- Transformations ! Au bout de 5 Mana le joueur peut décider de transformer son champion en sa version ELEMENTAL, pour tout défoncer. 
