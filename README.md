# ColorRedes
1. La lista está creada.

2. Están creados los métodos encargados de asignar el color y otro que genere aleatoriamente dichos colores.

3. En GameManager o HelloWorldManager está creado el método que contiene un botón,el cual cambiará: Si está en servidor cambia el color él, y si está en cliente, pediría al servidor el cambio de color.

## Cosas que no pude hacer:

- Cuando el juego como host y cliente, los colores se pueden cambiar en cada una de las pantallas,pero no se ven reflejadas entre ellas. Eso claramente es debido al metodo del serverRPC y en el update, de alguna manera necesito que en el servidor se vea los cambios del cliente y viceversa.

- Que los colores no estén ya asignados a un jugador. Como lo anterior no lo di rematado,aqui no pude continuar. Sin embargo intuyo que tenemos que hacer uso del NetworkList.

Problema principal de esta practica? Solo sé emplear las listas de una forma basica, estoy falto de conocimiento.
