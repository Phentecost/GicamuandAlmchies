Pablo, intente implementar la generacion de dungeon a la escena en donde 
estan los personajes pero no supe cuales eran los missing objects xd.
Entonces pa que muevas tus archivos a las carpetas pa que quede organizado y tal
thk.

ACLARACIONES
* Los puntos verdes debajo de los personajes son los "raycast", 
pa lo del coyote time (que cuando se termine de testear 
obviamente hay que ponerlos transparentes xd). Tambien pa que testees 
lo del tiempo del coyote time, creo que en 0.5s, teniendo en cuenta lo de 
las dimensiones que vamos a estar manejando y tal creo que ta bien.

* Le agrege lo de jump buffer, que guarda que si salto en el aire, 
antes de tocar el suelo, entonces que cuando toque el suelo, salte 
automaticamente, tambien lo puse en 0.5s, 
igualmente pa que lo testees a ver que tal.

* Le meti como velocidad dinamica, que el personaje empieza en velocidad 48 
y mientras que el jugador vaya avanzando la velocidad va sumando hasta que 
llegue a 80. Y cuando deja de avanzar, va mermando, hasta llegar a la 
velocidad minima. No supe la verdad cual quedaba mejor, si que se demorara 
menos tiempo en llegar a la velocidad maxima y mas tiempo en llegar a la 
velocidad minima o lo contrario. Que se demore mas tiempo en llegar a la 
velocidad maxima pero menos tiempo en llegar a la velocidad minima, 
pero lo mismo, mira y testea a ver.

* Tambien el salto es dinamico, o sea, si se deja presionado hace todo el recorrido,
hasta que empieza a caer, pero si se presiona por x tiempo, el personaje 
salta a esa x altura. Me gustaria que el salto como tal dure menos tiempo, 
no que sea menos alto, pero no supe la verdad como hacerlo, 
si puedes buscar o encuentras como hacerlo, seria muy bueno.

* Tambien le modifique la caida, cuando empieza a caer, la caida es mas 
rapida que cuando salta. Esta en 10, igual, testea.

* Aprovechando lo de la caida mas rapida puse que pulsando la s o la flecha 
abajo el personaje hiciera como que se agacha/cae mas rapido.

* Agrege como la mecanica de que el personaje se pueda tirar hacia abajo de 
una plataforma al estilo de las plataformas como con forma de puente de 
new super mario bros, que se pueden traspasar. Cosa "mala", hay que dejar 
el boton undido o sino se devuelve a su posicion original, o en el caso de que
el personaje halla pasado mas de la mitad del cuerpo por esa plataforma pues ya
si continua su recorrido. Si encuentras una forma de que solo haya que hundirlo 
sin necesidad de tener que estar presionando la tecla pues seria bueno que lo 
agregaras, y sino pues ya se inventa a nivel ux porque hay que tenerlo hundido xd.

* Puse que los personajes no se colisionaran entre si, igual si se colicionan 
contra los enemigos y demas objectos.

* En lo de las dugeon no se si van a haber avismos, pero seria bueno que si 
por X o Y el personaje sale volando fuera del cuarto, agreeges una deathZone, 
o que se teletransporte dentro del cuarto, pues es una recomendacion, no es necesario.

PD: Nunca habia pulido tanto un sistema de movimiento xd.

