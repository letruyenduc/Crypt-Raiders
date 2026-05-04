# Crypt Raiders - RPG Dungeon Crawler 2D

## 📜 Description du Projet
**Crypt Raiders** est un Action-RPG en 2D développé sous Unity, plongé dans une atmosphère mystique de **Désert et de Cryptes Anciennes**. Le joueur explore des tombeaux oubliés, combat des gardiens embaumés et progresse via un système de loot riche et structuré.

---

## 🏗️ Architecture Technique

### 1. Data-Driven Design (`ScriptableObjects`)
Le projet utilise une architecture basée sur les données pour faciliter l'équilibrage et l'extension du contenu.
- **ItemData** : Structure pivot définissant les Armes, Sorts et Armures.
- **ItemType** : `Arme_Melee`, `Arme_Magique`, `Casque`, `Plastron`, `Pantalon`, `Sort`.
- **Rareté** : `Commun` (Blanc), `Rare` (Bleu), `Epic` (Violet), `Legendaire` (Orange).
- **Scaling** : Système de calcul de dégâts basé sur la `Physique` ou la `Magie`.

### 2. Automatisation (Editor Scripts)
Pour garantir la cohérence des sets et gagner du temps, deux générateurs sont disponibles dans le menu **RPG** de l'éditeur Unity :
- **Générer Armures du Désert** : Crée 36 pièces d'armure réparties en 3 classes :
    - **Tank** (Set du Scarabée/Gardien) : Focus Max HP.
    - **Warrior** (Set d'Anubis/Pillard) : Équilibre HP / Bonus Physique.
    - **Mage** (Set du Vizir/Solaire) : Focus Bonus Magique.
- **Générer Armes et Sorts** : Crée la base offensive (Lames de bronze, Sceptres royaux, Souffles de momie, etc.).

*Convention de nommage des fichiers : `[Type]_[Rareté]_[Classe/Set]_[Nom].asset`*

### 3. Systèmes Core
- **Génération Procédurale** : Un `DungeonGenerator` assemble des salles (`Rooms`) dynamiquement pour créer un labyrinthe unique à chaque run.
- **IA de Combat** : Ennemis utilisant un système de **Pathfinding A*** personnalisé pour traquer le joueur dans les couloirs étroits de la crypte.
- **Health System** : Gestion robuste des points de vie avec feedbacks visuels (Hit flash) et gestion de la mort.

---

## 🏜️ Thématique : Désert & Crypte
Tout le contenu est visuellement et textuellement ancré dans cet univers :
- **Équipement** : Bandelettes d'embaumement, masques d'Anubis, linceuls de vizir, lames de soleil.
- **Ennemis** : Momies, scarabées dorés, gardiens de pierre.
- **Environnement** : Salles de sable, piliers gravés, éclairage tamisé de torches.

---

## 🛠️ Instructions pour les Développeurs

### Ajouter du contenu
1. Pour ajouter une nouvelle pièce d'armure, modifiez `ArmorDatabaseGenerator.cs` et relancez le script via `RPG > Générer Armures du Désert`.
2. Les icônes et descriptions peuvent être assignées directement sur les fichiers `.asset` générés dans `Assets/Items/`.

### Debug & Validation
- La console Unity affiche le succès des générations et l'état des salles lors du jeu.
- Vérifiez que les `LayerMasks` du `PathfindingGrid` sont correctement configurés pour les murs de la crypte.

---

## ✅ TODO List Prioritaire
- [ ] **Système d'Inventaire** : Interface UI pour équiper les 50+ objets générés.
- [ ] **Loot Drop UI** : Feedback visuel au sol lors de la défaite d'un boss.
- [ ] **Spell Manager** : Lier les ScriptableObjects de Sorts aux touches de raccourci (A/E).
