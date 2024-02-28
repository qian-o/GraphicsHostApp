project(GraphicsHostApp.OpenGL)

add_compile_options(-fPIC)

file(GLOB GRAPHICS_HEADERS "Graphics/*.h")
file(GLOB GRAPHICS_SOURCES "Graphics/*.cpp")

source_group("Graphics/Header Files" FILES ${GRAPHICS_HEADERS})
source_group("Graphics/Source Files" FILES ${GRAPHICS_SOURCES})

add_library(GraphicsHostApp.OpenGL SHARED ${GRAPHICS_SOURCES} ${GRAPHICS_HEADERS})

set(INTERFACE_SOURCES "GraphicsHostApp.OpenGL.cpp")
set(INTERFACE_HEADERS "GraphicsHostApp.OpenGL.h")

add_library(GraphicsHostApp.OpenGL SHARED ${INTERFACE_SOURCES} ${INTERFACE_HEADERS})

add_library(glad glad/src/glad.c)
target_include_directories(glad PUBLIC glad/include)
target_link_libraries(GraphicsHostApp.OpenGL glad)

# 在 CMake 版本大于 3.12 的情况下，设置 C++ 标准为 C++20
if (CMAKE_VERSION VERSION_GREATER 3.12)
  set_property(TARGET GraphicsHostApp.OpenGL PROPERTY CXX_STANDARD 20)
endif()
