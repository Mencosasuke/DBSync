$(document).ready(function(){

	var root = $("html").attr("data-root") + "/";

	// Workspaces MySQL y PostgreSQL
	var $workspace = $("#workspace").first();

    var events = {
    	initialize : function() {}
    };

    //$("#workarea").nextAll().not("script").css("display", "none");
    //$("li.start").siblings().css({"color": "red", "border": "2px solid red"});

    events.initialize = (function(){

		// Carga los eventos para los botones de mantenimiento y agreagar
		function eventosBotonesMantenimiento(){
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
		function cargarWorkspace(pagina){
			$.ajax({
				type: "POST",
				url: root + "DataBase/" + pagina,
				success: function(view) {
					// Inserta el contenido de la vista parcial en el workspace
					$workspace.html(view);

					// Activa los eventos de los botones del workspace
					eventosBotonesMantenimiento();
				}
			});
		};

		// Por defecto carga el workspace para MySQL
		cargarWorkspace("mysqlUpdateDelete");

    	// Acción de los tabas principales de MySQL y PostgreSQL

    	// Comportamiento del tab MySQL
    	$("#mysqlTab").on("click", function(){
    		var $this = $(this);

    		// Focus al tab correspondiente
    		$this.parent().siblings().removeClass("active");
			$this.parent().addClass("active");

			// Muestra el workspace para MySQL
			cargarWorkspace("mysqlUpdateDelete");

    	});

    	// Comportamiento del tab PostgreSQL
    	$("#pgsqlTab").on("click", function(){
    		var $this = $(this);
    		
    		// Focus al tab correspondiente
    		$this.parent().siblings().removeClass("active");
			$this.parent().addClass("active");

			// Muestra el workspace para PostgreSQL
			cargarWorkspace("pgsqlUpdateDelete");

    	});

    })();

});