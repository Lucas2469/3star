# 3Star - Mini-juegos estilo Among Us

Proyecto Unity con mini-juegos inspirados en Among Us para resolver tareas y desafíos.

## 📋 Descripción

Colección de mini-juegos interactivos desarrollados en Unity 6, cada uno representando diferentes tareas que los jugadores deben completar.

## 🎮 Mini-juegos Implementados

### 1. **NumberOrderTask** - Orden de Números
- **Objetivo**: Tocar los números del 1 al 10 en orden ascendente
- **Características**:
  - Orden aleatorio fijo que debe resolverse
  - Retroalimentación visual (normal, correcto, error)
  - Reinicio automático en caso de error
- **Escena**: `OrderNumber.unity`

### 2. **AlignLinesTask** - Alineación de Líneas
- **Objetivo**: Alinear las líneas giratorias usando la palanca
- **Características**:
  - Control preciso con slider
  - Indicadores de color (rojo/verde)
  - Tolerancia configurable
- **Escena**: `AlignTaskCube.unity`

### 3. **PhoneAlertsTask** - Alertas en el Teléfono
- **Objetivo**: Eliminar todas las alertas tocándolas 3 veces
- **Características**:
  - Efecto de escala al tocar
  - Estado BAD/GOOD
  - Sistema de alertas aleatorias
- **Escena**: `PhoneCubes.unity`

### 4. **ShieldTaskManager** - Tablero Hexagonal
- **Objetivo**: Apagar todos los hexágonos rojos
- **Características**:
  - Grid hexagonal generado proceduralmente
  - Posición inicial aleatoria
  - Sistema de apagado en cascada
- **Escena**: Integrado en otras escenas

### 5. **FuseTaskController** - Sistema de Fusibles
- **Objetivo**: Activar/desactivar el fusible
- **Características**:
  - Transición de estados con crossfade
  - Circuitos activos/inactivos
  - Animación suave

### 6. **DownloadTaskUI** - Simulación de Descarga
- **Objetivo**: Esperar a que complete la descarga
- **Características**:
  - Barra de progreso animada
  - Duración configurable
  - Feedback visual

## 🛠️ Tecnologías Utilizadas

- **Unity**: 6000.2.7f2 (Unity 6)
- **Render Pipeline**: Universal Render Pipeline (URP) 17.2.0
- **Input System**: 1.14.2
- **UI**: TextMeshPro para textos
- **Lenguaje**: C#

## 📁 Estructura del Proyecto

```
Assets/
├── 01_Scripts/           # Scripts de los mini-juegos
├── Art/                  # Assets visuales
├── Prefabs/              # Prefabs reutilizables
├── Scenes/               # Escenas de los juegos
└── TextMesh Pro/         # Fuentes y shaders de TextMeshPro
```

## 🚀 Scripts Principales

- `NumberOrderTask.cs` - Gestor de tarea de orden numérico
- `AlignLinesTask.cs` - Control de alineación
- `PhoneAlertsTask.cs` - Sistema de alertas
- `ShieldTaskManager.cs` - Tablero hexagonal
- `FuseTaskController.cs` - Control de fusibles
- `DownloadTaskUI.cs` - UI de descarga
- `NumberTile.cs`, `AlertTile.cs`, `HexTile.cs` - Componentes de UI

## 📝 Configuración

1. Abre el proyecto en Unity 6000.2.7f2 o superior
2. Las dependencias se instalarán automáticamente desde `Packages/manifest.json`
3. Abre cualquiera de las escenas en `Assets/Scenes/` para probar

## 🎯 Requisitos

- Unity 6000.2.7f2 o superior
- Universal Render Pipeline configurado
- Nuevo Input System habilitado

## 👥 Autores

- Lucas2469

## 📄 Licencia

Este proyecto es de código abierto.

