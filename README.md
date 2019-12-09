# Hell's Driver - Trabajo de fin de grado

El proyecto presentado es un videojuego de carreras de coches, donde la mayor parte del desarrollo ha sido relativo a la inteligencia artificial.
El videojuego proporciona suficiente variedad de contextos (circuitos, trazados, tipos de vehículos, ...). Uno de los aspectos prioritarios en el diseño del videojuego ha sido la incorporación de modelos adaptativos de Inteligencia Artificial para hacer atractivo para jugadores con niveles distintos de habilidad del usuario. Puesto que soy de la rama de Computación y Sistemas Inteligentes, la inteligencia artificial es la parte más destacable de este proyecto.

## Diseño y desarrollo
![imagen1](https://github.com/juliofgx17/HellsDriver-TFG/blob/master/capturas/imagen1.jpg)

## Gestión de escenas
![imagen2](https://github.com/juliofgx17/HellsDriver-TFG/blob/master/capturas/imagen2.jpg)

## Mecánicas de control del vehículo
![imagen3](https://github.com/juliofgx17/HellsDriver-TFG/blob/master/capturas/imagen3.jpg)

## Inteligencia Artificial de los vehículos
Recordemos el objetivo de la Inteligencia Artificial en cada vehículo: Completar el circuito en el menor tiempo posible. Para ello, la IA de un modelo debe proporcionarle al mecanismo de control de ese coche los valores necesarios para que este mecanismo de control lleve al coche a la meta.
En este proyecto, la solución empleada para encontrar una Inteligencia Artificial efectiva se divide en dos ramas: Para obtener el giro hemos usado una técnica puramente reactiva, mientras que para el freno y boost hemos usado una red neuronal entrenada con un algoritmo
genético, NEAT.

### IA para el giro del vehículo
El problema que debemos solucionar aquí es cuántos grados girar las ruedas de un vehículo para recorrer la trazada de una forma eficiente y sin colisionar con los bordes del circuito o (en la medida de lo posible) con otros vehículos.
En total tenemos un número de 22 sensores, separados en dos grupos: 11 en la esquina izquierda, y 11 en la esquina derecha. Primero cogemos el punto más lejano de cada grupo, comparamos, y nos quedamos con el menos lejano de los dos elegidos.
Si de los dos grupos escogemos hacer caso al grupo cuya máxima distancia sea menor, evitaremos siempre el choque.
Para que este sistema funcione, el circuito debe cumplir una condición: El ancho del circuito debe ser igual en todo su recorrido.
![imagen4](https://github.com/juliofgx17/HellsDriver-TFG/blob/master/capturas/imagen4.jpg)

### IA para el freno y boost del vehículo
El freno y el boost serán controlados por una red neuronal.
Esta red neuronal tiene como Outputs el freno, dado entre 0 y 2, y el boost, dado entre -1 y 1.
Como inputs, tenemos: Velocidad del vehículo, distancia respecto al borde del circuito, obtenida por cada uno de los sensores, y un input de bias.
El funcionamiento es, iterativamente, obtener los outputs de freno y boost, aplicarlos junto con la aceleración y giro, y obtener las nuevas distancias y velocidad.
![imagen5](https://github.com/juliofgx17/HellsDriver-TFG/blob/master/capturas/imagen5.jpg)

### Entrenamiento de la red neuronal: NEAT
Evolving Neural Networks throught Augmenting Topologies. Esto es NEAT. Que en español se traduce en evolucionar redes neuronales a través del aumento de la topología. Una cuestión importante en neuro evolución es cómo ganar ventaja evolucionando la topología de las redes neuronales además de sus pesos. NEAT es un método, que (según el autor) tiene gran eficiencia gracias a usar un método de crossover de diferentes topologías, proteger innovación estructural usando especiación, e ir incrementando gradualmente la topología. NEAT ofrece la posibilidad para los algoritmos genéticos de optimizar y complejizar soluciones simultáneamente, ofreciendo la posibilidad de evolucionar incrementando la complejidad de las soluciones generación tras generación. 

NEAT se ha implementado en C# usando el sistema de Scripts de Unity. La implementación la he hecho desde cero, tomando como referencia el paper del autor. Esencialmente la implementación es exactamente igual que la descrita por el autor en su paper, pero con un
sistema de crossover adicional. Además del sistema de crossover propuesto por el autor, se ha implementado (y usado) un crossover que consiste simplemente en, por cada individuo, obtener dos individuos: Una copia del mismo, y una copia mutada. Esto lo veremos más adelante.

### NEAT en nuestro proyecto
Aunque la implementación es esencialmente igual a la teoría expuesta anteriormente, vamos a intentar explicarla paso por paso para ver cómo hemos adaptado NEAT al entrenamiento de la IA de los vehículos.
#### Inicialización
Cada individuo de la población es una red neuronal que intentará conducir a su vehículo a la línea de meta sin colisiones de por medio. El primer paso del algoritmo, es crear esta población inicial de redes neuronales. Estas redes se crean de la forma más básica posible, sin capa de nodos ocultos, simplemente los inputs y outputs vistos anteriormente.

#### Obtención del fitness
Una vez tenemos nuestra población lista, vamos a obtener el fitness de cada individuo. El fitness se calcula de la siguiente manera: A toda la población de redes neuronales se le asigna un vehículo, siendo todos los vehículos del mismo modelo, y comienzan a correr el circuito a la vez. Los vehículos aparecen en el punto de salida, movidos hacia adelante o atrás, o izquierda o derecha una distancia aleatoria. El fitness es correlativo con el número de checkpoints cruzados: A mayor número, mayor fitness. Si el número es igual, tendrá mayor fitness el que más distancia haya recorrido desde el último checkpoint. Después multiplicamos el fitness por un peso. Este peso es el porcentaje de tiempo que el vehículo ha mantenido el acelerador a fondo.

#### Dividir en especies
El proceso de especiar es el mismo que el explicado en el apartado de NEAT, tenemos una distancia de compatibilidad, y unos representantes de las especies de la generación anterior. Para cada uno de los individuos, se comprueba su distancia con cada representante. En cuanto haya algún representante cuya distancia de compatibilidad sea menor al límite, se añade a esa especie. Si no hay ninguno, se crea una nueva especie.

#### Obtener los campeones
Como una medida de elitismo, y para no perder a los mejores individuos de cada especie, guardamos en una lista al mejor individuo de cada especie. Esta lista se usará más tarde.

#### Crossover o reproducción
Primero ordenamos a toda la población de mejor a peor individuo, y eliminamos a la mitad peor de toda la población. Ya podemos empezar el crossover.
Se han implementado dos formas de realizar el crossover.
Dada la forma original del autor, se escogen iterativamente parejas de la misma especie y se reproducen entre éstas, usando el método de crossover explicado anteriormente. Estos nuevos individuos se añaden a una nueva lista de hijos.
Dada la forma personalizada, no se usa el método de crossover del autor, si no que, para cada individuo, a la lista de hijos, se añade una copia idéntica, y otra mutada. En ambos casos se añaden a la lista de hijos los campeones obtenidos en la etapa anterior.

#### Mutación
La mutación puede ser de tres tipos: Añadir una conexión a la red, añadir una neurona, o modificar un peso.
El método de añadir una conexión consiste simplemente en conectar dos neuronas aleatoriamente, teniendo cuidado de que no se formen ciclos infinitos.
El método de añadir una neurona consiste en dividir una conexión que exista actualmente, e insertar una neurona en medio. La anterior conexión queda deshabilitada, y las nuevas conexiones tienen un peso de 1 la anterior y el mismo peso que la vieja, la posterior.
El método de modificar un peso consiste bien en cambiar el peso por otro totalmente aleatorio, o bien en modificarlo en un ligero porcentaje.

#### Reemplazo
Antes de reemplazar la población inicial por los hijos, se guardan representantes aleatorios de cada especie para especiar la generación siguiente. Una vez hecho esto, se reemplaza la población inicial, y volvemos al punto de partida con una generación un poco más evolucionada.

#### Finalización del entrenamiento
El entrenamiento termina cuando se alcanza un límite de generaciones, u opcionalmente, cuando consigamos individuos que completen el circuito, o cuando llevemos x generaciones con individuos que hayan completado el circuito.
Una vez acabado el entrenamiento, obtenemos el mejor individuo, y codificamos la red neuronal para que se guarde en un archivo de texto, y se pueda leer en cualquier momento. 

#### Esquema-resumen
![imagen6](https://github.com/juliofgx17/HellsDriver-TFG/blob/master/capturas/imagen6.jpg)


## Jugando al videojuego
![imagen7](https://github.com/juliofgx17/HellsDriver-TFG/blob/master/capturas/imagen7.jpg)
![imagen8](https://github.com/juliofgx17/HellsDriver-TFG/blob/master/capturas/imagen8.jpg)
![imagen9](https://github.com/juliofgx17/HellsDriver-TFG/blob/master/capturas/imagen9.jpg)
![imagen10](https://github.com/juliofgx17/HellsDriver-TFG/blob/master/capturas/imagen10.jpg)
![imagen11](https://github.com/juliofgx17/HellsDriver-TFG/blob/master/capturas/imagen11.jpg)
![imagen12](https://github.com/juliofgx17/HellsDriver-TFG/blob/master/capturas/imagen12.jpg)
![imagen13](https://github.com/juliofgx17/HellsDriver-TFG/blob/master/capturas/imagen13.jpg)
