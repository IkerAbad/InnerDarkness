# InnerDarkness

Juego 2D de sigilo y acción visto desde arriba, hecho con Unity 2020.3.47f1 (mayo de 2023, ciclo de DAM). El jugador debe llegar a la salida de cada nivel antes de que se acabe el tiempo, evitando a unos soldados que patrullan.

## Qué hay

- Seis escenas: menú principal, `Level1`, `Level2`, `FinalLevel`, pantalla de fin de partida (`GameOver`) y pantalla final (`TheEndMenu`).
- Movimiento del jugador con los ejes por defecto de Unity (teclas de dirección o WASD), animado en cuatro direcciones.
- Enemigos que recorren una ruta de puntos (`Waypoint`, `WaypointSystem`) con un cono de visión calculado con raycasts (`FOV.cs`). Al ver al jugador cambia la música, lo persiguen y le disparan.
- Vida del jugador (empieza en 10), objetos que suman vida, cuenta atrás por nivel que lleva a la pantalla de fin de partida al llegar a cero, e indicador de distancia a la salida.
- Menú de pausa con `Esc` y ajuste de volumen de música y efectos (`ConfigManager`).
- Scripts propios en `Assets/_Scripts` (17 ficheros C#).

No he abierto el proyecto en Unity ni lo he ejecutado desde aquí: solo he revisado el código y los ficheros.

## Cómo abrirlo

Con Unity Hub, instala la versión 2020.3.47f1 y abre la carpeta del repositorio. Las escenas están en `Assets/Scenes`.

## Licencia

MIT para mis scripts, ver [LICENSE](LICENSE). Las imágenes, la música, los efectos de sonido y los paquetes de Unity (como TextMesh Pro) no quedan cubiertos por esa licencia.

---

# InnerDarkness (English)

A top-down 2D stealth and action game made with Unity 2020.3.47f1 (May 2023, DAM vocational course). The player has to reach the exit of each level before time runs out, avoiding patrolling soldiers.

## What's inside

- Six scenes: main menu, `Level1`, `Level2`, `FinalLevel`, a game over screen (`GameOver`) and an ending screen (`TheEndMenu`).
- Player movement with Unity's default axes (arrow keys or WASD), animated in four directions.
- Enemies following a path of waypoints (`Waypoint`, `WaypointSystem`) with a vision cone computed with raycasts (`FOV.cs`). When they see the player the music changes, they chase him and shoot.
- Player health (starts at 10), pickups that add health, a per-level countdown that sends you to the game over screen at zero, and a distance-to-exit indicator.
- A pause menu with `Esc` and music and effects volume settings (`ConfigManager`).
- My own scripts in `Assets/_Scripts` (17 C# files).

I have not opened the project in Unity or run it from here: I only reviewed the code and the files.

## Opening it

With Unity Hub, install version 2020.3.47f1 and open the repository folder. The scenes are in `Assets/Scenes`.

## License

MIT for my scripts, see [LICENSE](LICENSE). The images, music, sound effects and Unity packages (such as TextMesh Pro) are not covered by that license.
