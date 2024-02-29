# 添加编译选项
add_compile_options(-fPIC)

# 包含OpenGL图形库的头文件和源文件
file(GLOB GRAPHICS_HEADERS "Graphics/*.h")
file(GLOB GRAPHICS_SOURCES "Graphics/*.cpp")

# 将头文件和源文件分组
source_group("Graphics/Header Files" FILES ${GRAPHICS_HEADERS})
source_group("Graphics/Source Files" FILES ${GRAPHICS_SOURCES})

# 创建名为GraphicsHostApp.OpenGL的动态链接库项目
add_library(GraphicsHostApp.OpenGL SHARED ${GRAPHICS_SOURCES} ${GRAPHICS_HEADERS})

# 设置接口源文件和头文件
set(INTERFACE_SOURCES "GraphicsHostApp.OpenGL.cpp")
set(INTERFACE_HEADERS "GraphicsHostApp.OpenGL.h")

# 添加glad库的源文件
add_library(glad glad/src/glad.c)

# 将glad库的包含目录添加到项目中
target_include_directories(glad PUBLIC glad/include)

# 将glad库链接到GraphicsHostApp.OpenGL项目中
target_link_libraries(GraphicsHostApp.OpenGL glad)

# 检查CMake版本并设置C++标准为C++20
if (CMAKE_VERSION VERSION_GREATER 3.12)
    set_property(TARGET GraphicsHostApp.OpenGL PROPERTY CXX_STANDARD 20)
endif()