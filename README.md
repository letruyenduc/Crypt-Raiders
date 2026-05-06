# Crypt Raiders - RPG Dungeon Crawler 2D

## 📜 Description du Projet
**Crypt Raiders** est un Action-RPG en 2D inspiré de *Dungeon Quest*, plongé dans une atmosphère mystique de **Désert et de Cryptes Anciennes**. Le joueur explore des tombeaux, gère son équipement, améliore ses statistiques et progresse à travers des difficultés croissantes.

---

## 🏗️ Architecture Technique

### 1. Systèmes de Progression & Stats
- **Leveling System** : Courbe d'XP exponentielle. Chaque niveau octroie 1 point de compétence.
- **Points de Statistique** : Attribution manuelle (Vie, Physique, Magique). Système de Reset payant en Or.
- **Équilibrage Core** : Player 100 HP / Enemies 50 HP / Base Damage 15.

### 2. Économie du Lobby (PNJ)
- **Système de Proximité** : Les interfaces s'ouvrent/se ferment automatiquement via `NPCTrigger`.
- **Le Marchand** : Vente d'objets du sac à dos contre de l'Or (prix basé sur la rareté et l'upgrade).
- **Le Forgeron** : Amélioration d'objets (+10% stats par niveau). Interface avec prévisualisation des gains (+X) et suivi de progression [Actuel/Max].
- **Gestionnaire d'Inventaire Dynamique** : La grille de l'inventaire se "téléporte" et s'adapte automatiquement aux menus des PNJ.

### 3. Système de Difficulté Dynamique
- **4 Niveaux** : Facile, Normal, Difficile, Cauchemar.
- **Scaling Automatique** : Multiplicateurs appliqués sur les PV ennemis, les Dégâts, l'XP, l'Or et la **quantité de Loot** (1 à 5 objets).
- **Mode Cauchemar** : Mode "Une seule vie" (La mort renvoie instantanément au Lobby sans respawn).

### 4. Mécaniques de Donjon & Survie
- **Dungeon Timer** : Limite de 5 minutes par donjon. Synchronisé avec le décompte de démarrage ("3, 2, 1, GO!").
- **Système de Respawn** : Checkpoints automatiques à l'entrée de chaque salle. 
- **Pénalités** : Mourir retire 30 secondes au timer global (sauf en Cauchemar).
- **Protection** : 2 secondes d'invincibilité avec effet de clignotement après un respawn.

### 5. Sauvegarde Persistante (`SaveManager`)
- **PlayerPrefs** : Pour les données simples (Niveau, Or, Points).
- **JSON Serialization** : Pour l'inventaire complet, préservant les statistiques aléatoires et les niveaux d'amélioration de chaque objet.
- **Auto-Save** : Sauvegarde lors des transitions de scènes et des Game Over.

### 6. Combat Feedback (VFX)
- **Dégâts Flottants** : Nombres sautant au-dessus des ennemis lors des impacts.
- **Damage Flash** : Flash rouge à l'écran lors de la réception de dégâts par le joueur.

---

## ✅ TODO List
- [x] **Système d'Inventaire** : Complet avec équipement et déséquipement.
- [x] **Progression & Stats** : XP, Niveaux et Points de compétence.
- [x] **Économie** : Marchand (Vente) et Forgeron (Upgrade).
- [x] **Navigation** : Lobby fonctionnel avec sélection de Map et Difficulté.
- [x] **Sauvegarde** : Système persistant JSON.
- [ ] **Visualisation de l'Équipement** : Changement du sprite joueur selon l'armure.
- [ ] **Feedback Impact (VFX)** : Particules et Screen Shake.
- [ ] **PNJ & Quêtes** : Ajout de dialogues et missions secondaires.
