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
		function eventosBotonesMantenimiento(panel){
			var $mantenimientoMySQL = $("#mantenimientoMySQL"),
				$nuevoMySQL = $("#nuevoMySQL"),
				$btnCancelarNuevoMySQL = $("#btnCancelarNuevoMySQL"),
				$panelMantenimientoMySQL = $("#panelMantenimientoMySQL"),
				$panelAddMySQL = $("#panelAddMySQL"),
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
			}

			$mantenimientoMySQL.on("click", function(){
				var $this = $(this);

				$this.parent().siblings().removeClass("active");
				$this.parent().addClass("active");

				$panelMantenimientoMySQL.removeClass("hidden");
				$panelAddMySQL.addClass("hidden");
			});
			
			$nuevoMySQL.on("click", function(){
				var $this = $(this);

				$this.parent().siblings().removeClass("active");
				$this.parent().addClass("active");

				$panelMantenimientoMySQL.addClass("hidden");
				$panelAddMySQL.removeClass("hidden");

				$("#txtDpi").attr("required", "required");
				$("#txtNombre").attr("required", "required");
				$("#txtApellido").attr("required", "required");
			});

			$btnCancelarNuevoMySQL.on("click", function(e){
				e.preventDefault();
				e.stopPropagation();

				$mantenimientoMySQL.parent().siblings().removeClass("active");
				$mantenimientoMySQL.parent().addClass("active");

				$panelMantenimientoMySQL.removeClass("hidden");
				$panelAddMySQL.addClass("hidden");
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