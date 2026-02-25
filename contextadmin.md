# 🛠️ Plan de Desarrollo: Backend Administrador (Área Admin)

Este documento contiene la hoja de ruta para desarrollar el panel de control del E-commerce de Calzado. El desarrollo sigue la arquitectura de 3 capas (Datos -> Negocio -> Presentación).



---

## 📦 Módulo 1: Gestión de Categorías
*Objetivo: Administrar las agrupaciones principales del calzado.*

### Capa de Negocio (BLL)
- [ ] Crear `ICategoriaService` (Contrato).
- [ ] Implementar `CategoriaService` con los métodos:
  - [ ] `ObtenerTodasAsync()`
  - [ ] `ObtenerPorIdAsync(int id)`
  - [ ] `CrearAsync(Categoria categoria)`
  - [ ] `ActualizarAsync(Categoria categoria)`
  - [ ] `EliminarAsync(int id)` *(Validar que no tenga productos asociados antes de borrar).*

### Capa de Presentación (MVC - Área Admin)
- [ ] Crear `CategoriaController`.
- [ ] Crear Vistas Razor: `Index` (Tabla), `Create`, `Edit`.

---

## 👟 Módulo 2: Gestión del Catálogo Maestro (Productos)
*Objetivo: Administrar los modelos de zapatos, sus detalles principales y ofertas.*

### Capa de Negocio (BLL)
- [ ] Crear `IProductoService`.
- [ ] Implementar `ProductoService` con los métodos:
  - [ ] `ObtenerTodosConCategoriaAsync()`
  - [ ] `ObtenerPorIdAsync(int id)`
  - [ ] `CrearAsync(Producto producto, IFormFile imagen)` *(Lógica para guardar la imagen en wwwroot/images).*
  - [ ] `ActualizarAsync(Producto producto, IFormFile nuevaImagen)`
  - [ ] `AplicarDescuentoAsync(int productoId, int porcentaje)` *(Regla: Validar porcentaje entre 0 y 100).*

### Capa de Presentación (MVC - Área Admin)
- [ ] Crear `ProductoController`.
- [ ] Crear ViewModels: `ProductoVM` (Para manejar la subida de imágenes y listas desplegables de categorías).
- [ ] Crear Vistas Razor: `Index`, `Upsert` (Crear/Editar unificado), `Detalle`.

---

## 🏷️ Módulo 3: Gestión de Inventario (Variantes)
*Objetivo: Controlar el stock real por talla y color para cada modelo de zapato.*

### Capa de Negocio (BLL)
- [ ] Crear `IProductoVarianteService`.
- [ ] Implementar `ProductoVarianteService` con los métodos:
  - [ ] `ObtenerVariantesPorProductoAsync(int productoId)`
  - [ ] `AgregarVarianteAsync(ProductoVariante variante)` *(Regla: Evitar duplicados exactos de talla/color en el mismo producto).*
  - [ ] `ActualizarStockAsync(int varianteId, int nuevoStock)`
  - [ ] `DesactivarVarianteAsync(int id)` *(Regla: Borrado lógico para proteger historial de compras).*

### Capa de Presentación (MVC - Área Admin)
- [ ] Crear `VarianteController` (Puede ser llamado desde la vista de Detalle del Producto).
- [ ] Crear Vistas Razor o Modales para gestionar el stock ágilmente.

---

## 🚚 Módulo 4: Gestión de Métodos de Envío
*Objetivo: Configurar las tarifas y tiempos de entrega.*

### Capa de Negocio (BLL)
- [ ] Crear `IMetodoEnvioService`.
- [ ] Implementar `MetodoEnvioService`:
  - [ ] CRUD Básico (Obtener, Crear, Editar, Eliminar).
  - [ ] *(Regla: Las ediciones solo aplican a pedidos futuros, no afectan el `CostoEnvioPagado` de los pasados).*

### Capa de Presentación (MVC - Área Admin)
- [ ] Crear `MetodoEnvioController`.
- [ ] Crear Vistas Razor: `Index`, `Create`, `Edit`.

---

## 🛒 Módulo 5: Gestión de Pedidos (Flujo Transaccional)
*Objetivo: Visualizar ventas y cambiar el estado de los pedidos.*

### Capa de Negocio (BLL)
- [ ] Crear `IPedidoAdminService`.
- [ ] Implementar `PedidoAdminService` con los métodos:
  - [ ] `ObtenerTodosLosPedidosAsync()`
  - [ ] `ObtenerDetallePedidoAsync(int pedidoId)`
  - [ ] `ActualizarEstadoPedidoAsync(int pedidoId, int nuevoEstado)` 
    - *(Regla RN-06: Validar máquina de estados. Ej: Creado -> Pagado -> Enviado).*
  - [ ] `CancelarPedidoAsync(int pedidoId)` 
    - *(Regla: Devolver el stock a las variantes de `ProductoVariante`).*

### Capa de Presentación (MVC - Área Admin)
- [ ] Crear `PedidoController`.
- [ ] Crear Vistas Razor: `Index` (Tabla con filtros por estado), `Detalle` (Para ver la factura, dirección y cambiar estados).