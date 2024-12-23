// Pre-processor include statements
#include <iostream>
#include <cmath>
#include <string>
#include <GL/glew.h>
#include <nlohmann/json.hpp>
#include <imgui/imgui.h>
#include <imgui/imgui_impl_glfw.h>
#include <imgui/imgui_impl_opengl3.h>

using namespace ImGui;

// Project header files
#include "../headers/SimSource.h"
#include "../headers/SimState.h"
#include "../headers/SimTimer.h"
#include "../headers/Window.h"
#include "../headers/StateLoader.h"

// Global variables
std::string projectPath;

int main(int argc, char** argv){

    // Get project path
    std::string fullpath = argv[0];
    projectPath = fullpath.substr(0, fullpath.find_last_of("/")) + "/..";

    // Initialize state objects
    WindowProps props;
    LoadWindow("empty", &props);
    SimState state(props.resolution);
    SimSource sources(&state);
    LoadState("empty", &state, &sources);

    // Set up simulation and control windows
    GLFWwindow* window = SimWindowSetup(props.resolution, props.winWidth);
    ControlWindowSetup(window, props.controlWidth);

    // Initialize timers
    SimTimer timer(props.maxFrameRate);
    timer.TrackFrameRatePerMS(1000);
    timer.StartSimulation();

    unsigned char* walldata = SetupWallTexture();

    // Simulation loop
    while(!glfwWindowShouldClose(window))
    {
        // Declare beginning of frame
        timer.StartFrame();

        // Draw current density to OpenGL window
        SimWindowRenderLoop(window, &state, &sources, walldata);
        // SimWindowRenderLoop(window, state.fields.dens, state.fields.temp, walldata);

        // Draw control window
        ControlWindowRenderLoop(window, &state, &sources, &timer, &props);

        // Update dynamic sources
        sources.UpdateSourcesDynamic();

        // Update simulation state
        state.SimulationStep(timer.DeltaTime());

        // Sleep until frame is complete
        timer.EndFrame();
    }

    delete[] walldata;

    CloseWindows(window);

    // Exit code
    return 0;
}
