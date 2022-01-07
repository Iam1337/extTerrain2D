# extTerrain2D - 2D Terrain for Unity
Created by [iam1337](https://github.com/iam1337)

![](https://img.shields.io/badge/unity-2021.1%20or%20later-green.svg)
[![⚙ Build and Release](https://github.com/Iam1337/extTerrain2D/actions/workflows/ci.yml/badge.svg)](https://github.com/Iam1337/extTerrain2D/actions/workflows/ci.yml)
[![openupm](https://img.shields.io/npm/v/com.iam1337.extterrain2d?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.iam1337.extterrain2d/)
[![](https://img.shields.io/github/license/iam1337/extTerrain2D.svg)](https://github.com/Iam1337/extTerrain2D/blob/master/LICENSE)
[![semantic-release: angular](https://img.shields.io/badge/semantic--release-angular-e10079?logo=semantic-release)](https://github.com/semantic-release/semantic-release)

## Introduction
extTerrain2D is a tool dedicated to create 2D terrains by beizer curve. It generates a mesh with two materials (two submesh) and colliders. Just look at [this video](https://www.youtube.com/watch?v=j7iCimyGXHo).

## Installation:

**Old school**

Just copy the [Assets/extTerrain2D](Assets/extTerrain2D) folder into your Assets directory within your Unity project, or [download latest extTerrain2D.unitypackage](https://github.com/iam1337/extTerrain2D/releases).

**OpenUPM**

Via [openupm-cli](https://github.com/openupm/openupm-cli):<br>
```
openupm add com.iam1337.extterrain2d
```

Or if you don't have it, add the scoped registry to manifest.json with the desired dependency semantic version:

```
"scopedRegistries": [
	{
		"name": "package.openupm.com",
		"url": "https://package.openupm.com",
		"scopes": [
			"com.iam1337.extterrain2d",
		]
	}
],
"dependencies": {
	"com.iam1337.extosc.extterrain2d": "1.0.0",
}
```

**Package Manager**

Project supports Unity Package Manager. To install the project as a Git package do the following:

1. In Unity, open **Window > Package Manager**.
2. Press the **+** button, choose **"Add package from git URL..."**
3. Enter "https://github.com/iam1337/extTerrain2D.git#upm" and press Add.

## Screenshots:
**Terrain2D**<br>
![Terrain2D](https://i.imgur.com/3js8FWa.png)
**CompoundTerrain2D**<br>
![CompoundTerrain2D](https://i.imgur.com/BC2f5R1.png)

**Inspector Editors**<br>
<img src="https://i.imgur.com/rPBWVH8.png" width="400"> <img src="https://i.imgur.com/B5OcA7p.png" width="400">

## Author Contacts:
\> [telegram.me/iam1337](http://telegram.me/iam1337) <br>
\> [ext@iron-wall.org](mailto:ext@iron-wall.org)

## License
This project is under the MIT License.
