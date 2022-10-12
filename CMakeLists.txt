cmake_minimum_required(VERSION 3.10)
set(TARGET_NAME ETWProcessor)
project(${TARGET_NAME} VERSION 0.1.0 LANGUAGES CSharp)

INCLUDE(CSharpUtilities)

file(GLOB ETW_PROCESSOR_SRCS ${CMAKE_CURRENT_SOURCE_DIR}/src/*)

include_directories(${CMAKE_CURRENT_SOURCE_DIR}/src)

add_executable(${TARGET_NAME} ${ETW_PROCESSOR_SRCS})

set_property(TARGET ${TARGET_NAME} 
    PROPERTY VS_PACKAGE_REFERENCES "Microsoft.Windows.EventTracing.Processing.All_1.10.0;"
)

