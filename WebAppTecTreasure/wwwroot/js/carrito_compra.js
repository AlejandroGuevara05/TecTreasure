$(document).ready(function () {
    // Puedes inicializar el contador con un valor inicial si es necesario
    let contador = 0;

    // Función para actualizar el contador
    function actualizarContador() {
        // Actualiza el texto del contador con el valor actual
        $("#contador-carrito").text(contador);
    }

    // Manejador de clics para los botones de "Comprar" en la página de Tienda
    $(".comprar-btn").click(function () {
        // Incrementa el contador cuando se hace clic en un botón de compra
        contador++;
        actualizarContador();
    });

    // Llamada de ejemplo para disminuir el contador en 1 (simulación)
    $("#disminuir-contador").click(function () {
        if (contador > 0) {
            contador--;
            actualizarContador();
        }
    });

    // Llama a actualizarContador para establecer el valor inicial del contador
    actualizarContador();
});
