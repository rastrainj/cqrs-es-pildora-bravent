# Píldora CQRS-ES (event sourcing)

## Infraestructura

### Iniciar contenedores Docker (postgres + pgadmin) con docker-compose:
`docker-compose -p cqrs-es-pildora up -d --build`

### Parar contenedores:
`docker-compose -p cqrs-es-pildora down`

## Casos de uso
Se va a desarrollar un sistema para gestión personal de las carreras de montaña.

La funcionalidad requerida es la siguiente:

- El corredor planificará una carrera en una fecha y hora futuras, en un determinado lugar y, opcionalmente, con una distancia y desnivel.
- El corredor podrá borrar una carrera mientras esté planificada.
- El corredor podrá modificar la fecha y la hora pero siempre a una fecha futura mientras esté planificada.
- El corredor podrá cambiar el lugar de la carrera mientras esté planificada.
- El corredor podrá marcar la distancia y/o desnivel de la carrera mientras esté planificada.
- El corredor podrá descartar la carrera mientras esté planificada.
- Cuando pase la fecha/hora de la carrera, ésta pasará al estado terminada.
- El corredor podrá marcar la carrera como realizada si ésta está terminada, indicando, si así lo desea, el tiempo invertido y/o la posición.
- No se podrá modificar la fecha/hora de una carrera ya terminada o realizada, solo en estado planificada.
- Se podrá cambiar el lugar y/o distancia y/o desnivel de la carrera en cualquier estado.

BONUS:

- Un corredor solo puede realizar cambios en las carreras que ha añadido, pero sí que puede ver las carreras de cualquier otro corredor.
- El corredor podrá filtrar sus carreras por estado, fecha, lugar, distancia y desnivel.
