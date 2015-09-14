$(document).ready(function(){

	var root = $("html").attr("data-root") + "/";

	// Workspaces MySQL y PostgreSQL
	var $workspace = $("#workspace").first();

	// Panel que debe ser cargado
	var panelToLoad = $workspace.attr("data-load");

    var events = {
    	initialize : function() {}
    };

    //$("#workarea").nextAll().not("script").css("display", "none");
    //$("li.start").siblings().css({"color": "red", "border": "2px solid red"});

    events.initialize = (function(){

		// Carga los eventos para los botones de mantenimiento y agreagar
		function eventosBotonesMantenimiento(panel, accion){
			var $mantenimientoMySQL = $("#mantenimientoMySQL"),
				$nuevoMySQL = $("#nuevoMySQL"),
				$modificarMySQL = $("#modificarMySQL"),
				$btnCancelarNuevoMySQL = $("#btnCancelarNuevoMySQL"),
				$btnCancelarModifyMySQL = $("#btnCancelarModifyMySQL"),
				$panelMantenimientoMySQL = $("#panelMantenimientoMySQL"),
				$panelAddMySQL = $("#panelAddMySQL"),
				$panelModifyMySQL = $("#panelModifyMySQL"),
				$mantenimientoPgSQL = $("#mantenimientoPgSQL"),
				$nuevoPgSQL = $("#nuevoPgSQL"),
				$btnCancelarNuevoPgSQL = $("#btnCancelarNuevoPgSQL"),
				$panelMantenimientoPgSQL = $("#panelMantenimientoPgSQL"),
				$panelAddPgSQL = $("#panelAddPgSQL");

			// Defino los estilos active para los elementos segun el panel cargado
			if(panelToLoad === "pgsql"){
				$("#pgsqlTab").parent().siblings().removeClass("active");
				$("#pgsqlTab").parent().addClass("active");
			}else{
				$("#mysqlTab").parent().siblings().removeClass("active");
				$("#mysqlTab").parent().addClass("active");
				
				if(accion === "modificar"){
					$modificarMySQL.parent().siblings().removeClass("active");
					$modificarMySQL.parent().addClass("active");
					$modificarMySQL.parent().removeClass("disabled");

					$panelMantenimientoMySQL.addClass("hidden");
					$panelAddMySQL.addClass("hidden");
					$panelModifyMySQL.removeClass("hidden");
				}
			}

			$mantenimientoMySQL.on("click", function(){
				var $this = $(this);

				$this.parent().siblings().removeClass("active");
				$this.parent().addClass("active");
				$modificarMySQL.parent().addClass("disabled");

				$panelMantenimientoMySQL.removeClass("hidden");
				$panelAddMySQL.addClass("hidden");
				$panelModifyMySQL.addClass("hidden");

				$modificarMySQL.unbind("click");
			});
			
			$nuevoMySQL.on("click", function(){
				var $this = $(this);

				$this.parent().siblings().removeClass("active");
				$this.parent().addClass("active");
				$modificarMySQL.parent().addClass("disabled");

				$panelMantenimientoMySQL.addClass("hidden");
				$panelAddMySQL.removeClass("hidden");
				$panelModifyMySQL.addClass("hidden");

				$("#txtDpi").attr("required", "required");
				$("#txtNombre").attr("required", "required");
				$("#txtApellido").attr("required", "required");

				$modificarMySQL.unbind("click");
			});

			if(accion === "modificar"){
				$modificarMySQL.on("click", function(){
					var $this = $(this);

					$this.parent().siblings().removeClass("active");
					$this.parent().addClass("active");
					$this.parent().removeClass("disabled");

					$panelMantenimientoMySQL.addClass("hidden");
					$panelAddMySQL.addClass("hidden");
					$panelModifyMySQL.removeClass("hidden");
				});

				accion = "";
			}else{
				$modificarMySQL.unbind("click");
			}

			$btnCancelarNuevoMySQL.on("click", function(e){
				e.preventDefault();
				e.stopPropagation();

				$mantenimientoMySQL.parent().siblings().removeClass("active");
				$mantenimientoMySQL.parent().addClass("active");

				$panelMantenimientoMySQL.removeClass("hidden");
				$panelAddMySQL.addClass("hidden");
			});

			$btnCancelarModifyMySQL.on("click", function(e){
				e.preventDefault();
				e.stopPropagation();

				$mantenimientoMySQL.parent().siblings().removeClass("active");
				$mantenimientoMySQL.parent().addClass("active");

				$panelMantenimientoMySQL.removeClass("hidden");
				$panelAddMySQL.addClass("hidden");
				$panelModifyMySQL.addClass("hidden");
			});

			$mantenimientoPgSQL.on("click", function(){
				var $this = $(this);

				$this.parent().siblings().removeClass("active");
				$this.parent().addClass("active");

				$panelMantenimientoPgSQL.removeClass("hidden");
				$panelAddPgSQL.addClass("hidden");
			});

			$nuevoPgSQL.on("click", function(){
				var $this = $(this);

				$this.parent().siblings().removeClass("active");
				$this.parent().addClass("active");

				$panelMantenimientoPgSQL.addClass("hidden");
				$panelAddPgSQL.removeClass("hidden");

				$("#txtDpi").attr("required", "required");
				$("#txtNombre").attr("required", "required");
				$("#txtApellido").attr("required", "required");
			});

			$btnCancelarNuevoPgSQL.on("click", function(e){
				e.preventDefault();
				e.stopPropagation();

				$mantenimientoPgSQL.parent().siblings().removeClass("active");
				$mantenimientoPgSQL.parent().addClass("active");

				$panelMantenimientoPgSQL.removeClass("hidden");
				$panelAddPgSQL.addClass("hidden");
			});

		};

		// Acción para el botón de modificar contacto MySQL
		function ActivarBotonesModificar(){
			var $btnModificarContactoMySQL = $(".btnModificarContactoMySQL");

			$btnModificarContactoMySQL.each(function(){
				var $this = $(this);

				$this.on("click", function(){
					var contacto = JSON.parse($this.attr("data-id"));
					//var contacto = $this.attr("data-id");

					var newContacto = {
						dpi : contacto.dpi,
						nombre : contacto.nombre,
						apellido : contacto.apellido,
						direccion : contacto.direccion,
						telefonoCasa : contacto.telefonoCasa,
						telefonoMovil : contacto.telefonoMovil,
						nombreContacto : contacto.nombreContacto,
						numeroContacto : contacto.numeroContacto
					};
					console.log(newContacto);
					$.ajax({
						type: "POST",
						url: root + "DataBase/mysqlUpdateDelete",
						contentType: "application/json",
						data: JSON.stringify({ modContacto : newContacto }),
						dataType: 'html',
						success: function(view) {
							// Inserta el contenido de la vista parcial en el workspace
							$workspace.html(view);
							console.log("evento lanzado");
							// Activa los botones para eliminar y modificar
							eventosBotonesMantenimiento("mysql", "modificar");

							// Activa los botones para eliminar y modificar
							ActivarBotonesModificar();
						}
					});
				});

			});
		};

		// Funcion para cargar el contenido de un workspace espeficico
		function cargarWorkspace(pagina, panel){
			$.ajax({
				type: "POST",
				url: root + "DataBase/" + pagina,
				success: function(view) {
					// Inserta el contenido de la vista parcial en el workspace
					$workspace.html(view);

					// Activa los eventos de los botones del workspace
					eventosBotonesMantenimiento(panel);

					// Activa los botones para eliminar y modificar
					ActivarBotonesModificar();
				}
			});
		};

		// Carga el workspace especificado, si no se ha definido uno, por defecto carga MySQL
		if(panelToLoad === "pgsql"){
			cargarWorkspace("pgsqlUpdateDelete");
		}else{
			cargarWorkspace("mysqlUpdateDelete");
		}

    	// Acción de los tabas principales de MySQL y PostgreSQL

    	// Comportamiento del tab MySQL
    	$("#mysqlTab").on("click", function(){
    		var $this = $(this);

    		// Focus al tab correspondiente
    		$this.parent().siblings().removeClass("active");
			$this.parent().addClass("active");

			panelToLoad = "mysql";
			$workspace.attr("data-load", panelToLoad);

			// Muestra el workspace para MySQL
			cargarWorkspace("mysqlUpdateDelete");

    	});

    	// Comportamiento del tab PostgreSQL
    	$("#pgsqlTab").on("click", function(){
    		var $this = $(this);
    		
    		// Focus al tab correspondiente
    		$this.parent().siblings().removeClass("active");
			$this.parent().addClass("active");

			panelToLoad = "pgsql";
			$workspace.attr("data-load", panelToLoad);

			// Muestra el workspace para PostgreSQL
			cargarWorkspace("pgsqlUpdateDelete");

    	});

    })();

});