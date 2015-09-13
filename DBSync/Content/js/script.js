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

		// Funcion para cargar el contenido de un workspace espeficico
		function cargarWorkspace(pagina){
			$.ajax({
				type: "POST",
				url: root + "Data/" + pagina,
				success: function(view) {
					$workspace.html(view);
				}
			});
		}

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

    	// Comportamiento de los contenidos de 

    })();

});