cmake_minimum_required(VERSION 3.20)
project(Fuzr)

# === User Configurable Settings ===
set(FUZR_ASSEMBLY_NAME "FuzrExample")
set(SCRIPTS_VERSION "1.0.0")
set(SCRIPTS_PATH_1 "${CMAKE_SOURCE_DIR}/Scripts")
#set(SCRIPTS_PATH_2 "${CMAKE_SOURCE_DIR}/MoreScripts")
###
set(FUZR_BUILD_PATH "${CMAKE_SOURCE_DIR}/Fuzr.Core/Fuzr.Build")
set(FUZR_CORE_PATH "${CMAKE_SOURCE_DIR}/Fuzr.Core/Fuzr.Core")
set(FUZR_CORE_APPS_PATH "${FUZR_CORE_PATH}/apps")
set(FUZR_BUILD_ARGS "--output" ${FUZR_CORE_APPS_PATH} "--apps-path" "${SCRIPTS_PATH_1}")
#set(FUZR_BUILD_ARGS "--output" ${FUZR_CORE_APPS_PATH} "--apps-path" "${SCRIPTS_PATH_1}" "--apps-path" "${SCRIPTS_PATH_2}") # Example multiple scripts folder
set(FUZR_CORE_REPO "https://github.com/Fuzr-Project/Fuzr.Core.git")
set(TEMP_DIR "${CMAKE_BINARY_DIR}/CMakeTmp")
set(FUZR_BUILD_ARTIFACT_PATH "${TEMP_DIR}/Fuzr.Build/build")
set(FUZR_BUILD_OUTPUT_PATH "${TEMP_DIR}/Fuzr.Build")
set(FUZR_CORE_ARTIFACT_PATH "${TEMP_DIR}/Fuzr.Core/build")
set(FUZR_CORE_OUTPUT_PATH "${CMAKE_SOURCE_DIR}/bin")
# === End User Configurable Settings ===

# Clone if missing
find_program(GIT_EXECUTABLE git REQUIRED)

if(NOT EXISTS ${FUZR_CORE_PATH})
    message(STATUS "Cloning Fuzr.Core from ${FUZR_CORE_REPO}...")
    execute_process(
        COMMAND ${GIT_EXECUTABLE} clone ${FUZR_CORE_REPO} ${CMAKE_SOURCE_DIR}/Fuzr.Core
        WORKING_DIRECTORY ${CMAKE_SOURCE_DIR}
        RESULT_VARIABLE GIT_CLONE_RESULT
    )
    if(NOT GIT_CLONE_RESULT EQUAL 0)
        message(FATAL_ERROR "Failed to clone Fuzr.Core from ${FUZR_CORE_REPO}")
    endif()
endif()

# Check dotnet
find_program(DOTNET_EXECUTABLE dotnet)

if(NOT DOTNET_EXECUTABLE)
    message(FATAL_ERROR "dotnet CLI not found!")
endif()

# Check dotnet version (must be 10 or above)
execute_process(
    COMMAND ${DOTNET_EXECUTABLE} --version
    OUTPUT_VARIABLE DOTNET_VERSION
    OUTPUT_STRIP_TRAILING_WHITESPACE
)

string(REGEX MATCH "^[0-9]+" DOTNET_MAJOR_VERSION ${DOTNET_VERSION})

if(NOT DOTNET_MAJOR_VERSION)
    message(FATAL_ERROR "Could not parse dotnet version: ${DOTNET_VERSION}")
endif()

if(DOTNET_MAJOR_VERSION LESS 10)
    message(FATAL_ERROR "dotnet version must be 10 or higher. Found: ${DOTNET_VERSION}")
endif()

message(STATUS "dotnet version ${DOTNET_VERSION} detected, proceeding...")

# Build actions
add_custom_target(fuzr_build
    COMMAND ${DOTNET_EXECUTABLE} build -c Release --artifacts-path ${FUZR_BUILD_ARTIFACT_PATH} --output ${FUZR_BUILD_OUTPUT_PATH}
    WORKING_DIRECTORY ${FUZR_BUILD_PATH}
    COMMENT "Compiling Fuzr.Build"
)

add_custom_target(run_fuzr_build
    COMMAND ${CMAKE_COMMAND} -E rm -rf ${FUZR_CORE_APPS_PATH}
    COMMAND ${FUZR_BUILD_OUTPUT_PATH}/Fuzr.Build ${FUZR_BUILD_ARGS}
    DEPENDS fuzr_build
    WORKING_DIRECTORY ${FUZR_BUILD_OUTPUT_PATH}
    COMMENT "Compiling scripts"
)

add_custom_target(build_fuzr_core
    COMMAND ${DOTNET_EXECUTABLE} publish -c Release /p:AssemblyName=${FUZR_ASSEMBLY_NAME} /p:ScriptsVersion=${SCRIPTS_VERSION} /p:DebugType=None /p:CopyOutputSymbolsToPublishDirectory=false --artifacts-path ${FUZR_CORE_ARTIFACT_PATH} --output ${FUZR_CORE_OUTPUT_PATH}
    WORKING_DIRECTORY ${FUZR_CORE_PATH}
    COMMENT "Compiling Fuzr.Core"
)

# Build steps
add_dependencies(run_fuzr_build fuzr_build)

add_dependencies(build_fuzr_core run_fuzr_build)

add_custom_target(build_all
    ALL
    DEPENDS build_fuzr_core
)