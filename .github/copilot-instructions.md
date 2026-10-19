# GitHub Copilot Instructions for RimWorld Mod: Perspective: Eaves (Continued)

## Mod Overview and Purpose

**Mod Name:** Perspective: Eaves (Continued)  
**Author:** Originally by Owlchemist, maintained by Mlie  
**Package ID:** Mlie.PerspectiveEaves

The "Perspective: Eaves (Continued)" mod addresses a graphical issue in RimWorld where outdoor roof shadows are not rendered correctly. Normally, the eaves of roofs are ignored by the sun shadow caster, causing shadows to appear incorrectly along the outside of buildings. This mod corrects that behavior by ensuring that roof shadows render properly, improving the visual consistency of structures in the game.

## Key Features and Systems

- **Shadow Correction for Roof Eaves:** Ensures that all outdoor roofs cast correct shadows, resolving visual inconsistencies.
- **Compatibility:** This is a non-intrusive graphical mod that can be added or removed without affecting saved games and is compatible with most other roof mods.
- **Performance:** The mod efficiently updates shadowing only when necessary, specifically when the map is redrawn, without impacting game performance.

## Coding Patterns and Conventions

### C# Coding Practices
- **File Structure:** The source directory contains seven C# files with descriptive names that reflect their functionalities, e.g., `RoofShadows.cs`, `HarmonyPatches.cs`.
- **Class Naming:** Classes are named to succinctly describe their purpose, such as `RoofShadows` for handling shadow adjustments.
- **Method Naming:** Methods are implemented to clearly delineate behavior, such as `GetAdjustedList`, `MapMeshDirty`, and `OutdoorRoofed`.

### XML Integration

- **Configuration:** The mod uses an `About.xml` file for metadata, ensuring compatibility with the game's mod loader and dependency on Harmony (brrainz.harmony).

## Harmony Patching

- **Harmony Integration:** The mod relies on Harmony, a popular library for patching methods at runtime, allowing modification of base game code without altering original files.
- **Patch Implementation:**
  - **Transpiler Usage:** Modifies the method instructions, as observed in `RoofGrid_SetRoof.cs` and `Building_SpawnSetup.cs`.
  - **Prefix Methods:** Used in `SectionLayer_IndoorMask_HideRainFogOverlay.cs` to execute code before the original method.
- **HarmonyPatches Class:** Centralizes and organizes all Harmony patches for easier management and readability.

## Suggestions for Copilot

1. **Code Completion:** Suggest code completions that follow the existing naming conventions and structure used in this mod.
2. **Patch Suggestions:** Provide examples or templates for Harmony patches, especially when new behavioral changes require method alteration.
3. **Performance Optimization:** Offer code snippets that maintain or improve performance, especially regarding rendering updates.
4. **Debugging Assistance:** Suggest logging practices that comply with RimWorld's modding community standards for easier troubleshooting.
5. **XML Templates:** When applicable, suggest templates for XML configuration that maintain consistency with the existing `About.xml`.

By following these guidelines and leveraging GitHub Copilot, developers can effectively maintain and expand the "Perspective: Eaves (Continued)" mod, ensuring ongoing compatibility and enhancement of RimWorld's graphical presentation.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

