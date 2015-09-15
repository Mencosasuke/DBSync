--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.1
-- Dumped by pg_dump version 9.4.1
-- Started on 2015-09-15 15:55:24

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

DROP DATABASE "DBSync";
--
-- TOC entry 2000 (class 1262 OID 16393)
-- Name: DBSync; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "DBSync" WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'Spanish_Guatemala.1252' LC_CTYPE = 'Spanish_Guatemala.1252';


ALTER DATABASE "DBSync" OWNER TO postgres;

\connect "DBSync"

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 5 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA public;


ALTER SCHEMA public OWNER TO postgres;

--
-- TOC entry 2001 (class 0 OID 0)
-- Dependencies: 5
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON SCHEMA public IS 'standard public schema';


--
-- TOC entry 174 (class 3079 OID 11855)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2003 (class 0 OID 0)
-- Dependencies: 174
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 172 (class 1259 OID 16394)
-- Name: contacto; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE contacto (
    dpi character varying(20) NOT NULL,
    nombre character varying(50),
    apellido character varying(50),
    direccion character varying(60),
    telefono_casa character varying(20),
    telefono_movil character varying(20),
    nombre_contacto character varying(50),
    numero_telefono_contacto character varying(20)
);


ALTER TABLE contacto OWNER TO postgres;

--
-- TOC entry 173 (class 1259 OID 24576)
-- Name: usuario; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE usuario (
    usuario character varying(20),
    password character varying(50),
    rol character varying(20)
);


ALTER TABLE usuario OWNER TO postgres;

--
-- TOC entry 1994 (class 0 OID 16394)
-- Dependencies: 172
-- Data for Name: contacto; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 1995 (class 0 OID 24576)
-- Dependencies: 173
-- Data for Name: usuario; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO usuario (usuario, password, rol) VALUES ('dmencos', 'test', 'usuario');
INSERT INTO usuario (usuario, password, rol) VALUES ('admin', 'test', 'administrador');


--
-- TOC entry 1884 (class 2606 OID 16398)
-- Name: pk_dpi; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY contacto
    ADD CONSTRAINT pk_dpi PRIMARY KEY (dpi);


--
-- TOC entry 2002 (class 0 OID 0)
-- Dependencies: 5
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2015-09-15 15:55:24

--
-- PostgreSQL database dump complete
--

