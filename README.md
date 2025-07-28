# MiProyecto
MiProyecto - Tienda y Plataforma de Intercambio de Cartas Pokémon

Descripción

MiProyecto es una aplicación web basada en el modelo MVC (Modelo-Vista-Controlador) que permite a los usuarios gestionar, comprar e intercambiar cartas Pokémon. Los usuarios pueden crear colecciones, comprar cartas en una tienda virtual, realizar intercambios con otros usuarios y ver detalles de cartas y colecciones. La plataforma distingue entre dos roles: Administradores y Usuarios Comunes, con permisos diferenciados para garantizar una experiencia segura y controlada.

Características Principales

Gestión de Usuarios:
Administradores: Pueden realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) sobre usuarios, cartas, colecciones y compras.
Usuarios Comunes: Solo pueden editar su propio perfil (email, nombre, apellido, DNI, avatar) y gestionar sus puntos virtuales.



Tienda Virtual:
Los usuarios pueden comprar cartas usando puntos virtuales.
Las cartas en la tienda tienen un precio definido y un stock ilimitado, gestionado por el administrador.
Los administradores pueden agregar o retirar cartas de la tienda.



Intercambios:

Los usuarios pueden iniciar intercambios de cartas (hasta 5 cartas por usuario por intercambio).
El oferente selecciona una carta o cartas de la colección del receptor .
El receptor revisa el intercambio, puede aceptar (eligiendo cartas del oferente), rechazar o cancelar.
El oferente confirma o cancela tras la aceptación del receptor.
Si ambas partes confirman, las cartas se transfieren a las colecciones respectivas.
las transacciones solo estan abiertas por tiempo limitado y solo peude haber una transaccion abierta por par de usuarios.



Colecciones:

Los usuarios pueden crear y gestionar colecciones de cartas.
Las colecciones son públicas (visibles para otros)
os usuarios comunes solo pueden ver detalles de cartas y colecciones públicas de otros usuarios.



Historial:
Los usuarios pueden ver su propio historial de intercambios y compras.



Restricciones:


Los usuarios comunes no pueden editar ni eliminar cartas, colecciones ni compras, solo visualizarlas.
solo los administradores tienen control total sobre todas las entidades.
