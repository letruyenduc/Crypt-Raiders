# Crypt Raiders - RPG Dungeon Crawler 2D

## 📜 Description du Projet
Crypt Raiders est un Action-RPG en 2D développé sous Unity. Le joueur explore un donjon généré de manière procédurale, combat des ennemis dotés d'une IA avancée, et progresse via un système de loot et d'équipement structuré par classes (Guerrier/Mage).

---

## 🏗️ Architecture Technique Actuelle

### 1. Système de Donjon & Salles (`Rooms`)
- **DungeonGenerator** : Singleton gérant le spawn procédural des salles à partir d'un pool de prefabs.
- **RoomManager** : Gère l'état d'une salle (Combat/Nettoyée). Verrouille les portes à l'entrée et les déverrouille une fois les ennemis vaincus.
- **StartRoomController** : Gère la phase d'initialisation (UI de démarrage, compte à rebours, ouverture du donjon).

### 2. Système RPG & Data (`Data` & `Editor`)
- **ItemData (ScriptableObject)** : Définition de base des objets (Armes, Armures, Sorts) avec rareté, statistiques et scaling.
- **ItemInstance** : Logique de "God Roll" gérant la variance des stats (+/- 10%) et le potentiel d'amélioration (Upgrades) basé sur la rareté.
- **Générateurs Automatisés** :
    - `WeaponSpellDatabaseGenerator` : Génère les dagues, épées, bâtons, orbes et sorts (Physique/Magique).
    - `ArmorDatabaseGenerator` : Génère 36 pièces d'armure (Casque, Plastron, Pantalon) réparties par thèmes (Tank, Guerrier, Mage).

### 3. Intelligence Artificielle & Navigation (`Core`)
- **Pathfinding A*** : Algorithme personnalisé basé sur une grille (`PathfindingGrid`) pour une navigation fluide des ennemis en 2D.
- **SimpleEnemyFollow** : IA de poursuite utilisant le chemin calculé pour traquer le joueur.

### 4. Gestion de l'État de Jeu (`Core` & `UI`)
- **Health System** : Gestion de la vie pour le joueur et les ennemis avec feedbacks visuels (flash rouge).
- **VictoryManager** : Gère les conditions de victoire et le passage aux niveaux suivants.
- **GameOverManager** : Gère la défaite du joueur et le retour au menu/restart.

---

## ✅ État d'Avancement (TODO List)

### 🌍 Monde & Progression
- [x] Système de spawn et génération de salles.
- [x] Conditions de Victoire / Défaite.
- [ ] **Sélection de la Map** (Menu pour choisir entre 2 thèmes visuels).
- [ ] **Difficultés** (Facile → Nightmare) influençant le nombre de salles et la puissance des ennemis.
- [ ] **Limite de Salles** dynamique selon la difficulté.

### ⚔️ Combat & Équipement
- [x] 2 Classes d'objets (Guerrier/Mage).
- [x] 4 Raretés (Commun, Rare, Epic, Légendaire).
- [x] Sets d'armures complets (Casque, Plastron, Pantalon).
- [x] Sorts avec scaling Physique ou Magique.
- [ ] **Slots de Sorts (A et E)** : Système de lancement de sorts assigné aux touches.

### 🎒 Systèmes RPG Avancés
- [ ] **Inventaire & Équipement UI** : Interface pour visualiser et équiper le loot.
- [ ] **Loot Système** : Drop d'objets uniquement sur les Boss avec affichage visuel du drop.
- [ ] **Système de Niveau** : Gain d'XP et points de statistiques à répartir (Force/Magie/Vie).
- [ ] **Polissage** : Intégration complète des VFX, SFX et Musiques d'ambiance.

---

## 🚀 Prochaines Étapes Recommandées
1. **Implémenter l'Inventaire** : Crucial pour utiliser les 50+ objets déjà générés.
2. **Système de Difficulté** : Connecter le `DungeonGenerator` à un paramètre de difficulté pour limiter le nombre de salles.
3. **Lanceur de Sorts** : Créer le script `SpellCaster` pour lier les ScriptableObjects `Sort` aux touches A et E.

---

## 🛠️ Instructions Développeur
- **Génération des Data** : Utilisez le menu `RPG > Générer ...` pour reconstruire la base de données d'objets.
- **Debug** : Les logs de console indiquent les points de spawn et les états de nettoyage des salles.
