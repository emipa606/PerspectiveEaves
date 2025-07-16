# .github/copilot-instructions.md

## Mod Overview and Purpose

The Perspective Eaves mod for RimWorld enhances the visual realism by adjusting the appearance of roofs and shadows within the game. It focuses on providing a more immersive experience by dynamically altering how roofs and their shadows interact with the environment, particularly concerning weather effects and indoor visibility.

## Key Features and Systems

- **Dynamic Roof Visualization**: The mod introduces a system that alters how roofs are visualized under various in-game conditions.
- **Shadow Adjustments**: Modifies sun shadows based on eave placement and structure orientation.
- **Weather Interaction**: Incorporates weather conditions into roof and shadow dynamics, such as hiding rain and fog overlays when viewing indoor spaces.
- **Seamless XML Integration**: Utilizes XML for data-driven mod settings and definitions.
- **Harmony Patching**: Applies runtime modifications to the game using the Harmony library to ensure compatibility and integration without directly altering the base game code.

## Coding Patterns and Conventions

- **Class Naming**: Uses PascalCase for class names, e.g., `Building_SpawnSetup`, `RoofShadows`.
- **Method Naming**: Methods follow PascalCase.
- **Access Modifiers**: Conventions like `internal` for classes such as `HarmonyPatches` to limit exposure of implementation details.
- **Static Utility Classes**: Utilizes static classes like `RoofShadows` for shared functionality that does not require instance creation.

## XML Integration

- Use XML for defining mod settings and configurations to allow dynamic changes without recompiling.
- XML files can store data like roof types, shadow settings, or visibility options.
- Ensure XML files are well-structured and validated to prevent runtime errors.

## Harmony Patching

- **Patch Classes**: Utilize classes like `HarmonyPatches` to apply Harmony patches.
- **Method Patching**: Use prefixes, postfixes, or transpilers to modify existing game methods non-destructively.
- **Namespace Utilization**: Follow proper namespace organization to keep patches organized and prevent conflicts.

## Suggestions for Copilot

- **Code Completion**: Assist with auto-generating boilerplate code for new patches or adding new features.
- **Error Prediction**: Suggest fixes for potential runtime or compile-time issues commonly encountered in mod development.
- **Refactoring Assistance**: Offer suggestions for refactoring and optimizing existing code, enhancing readability and maintainability.
- **XML Structure Generation**: Create templates for XML structure used within RimWorld mods, making integration straightforward.
- **Debugging Commands**: Generate debugging aids and suggest logging snippets for enhanced troubleshooting during mod development.
- **Harmony Examples**: Provide examples of Harmony patching patterns that align with current modding requirements, like method postfixes or transpile instructions.
