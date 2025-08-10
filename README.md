# ✨ What is Fuzr?

**Fuzr** is a wrapper, collection, or container (call it whatever you like) for **.NET 10 file-based apps**.  

If you haven’t heard yet, starting from .NET 10, Microsoft introduced **file-based apps** — also known as scripting mode.  
This means you can execute a single `.cs` file without needing a `.csproj` or any other project structure.  

When I first discovered this feature, I was amazed! For someone like me who works across multiple platforms, this makes scripting much easier. Normally, I’d use **PowerShell** for Windows and **Bash** for Linux/macOS, but now I can write scripts once in C# and run them anywhere.  

That’s where **Fuzr** comes in — it compiles all your scripts into a **single executable**. You can simply copy that one file, and inside it, you’ve got a bunch of scripts ready to go.  

**Example use case:**  
A DevOps engineer wants to run scripts on a server without installing anything. With Fuzr, you can run those scripts because the compiled binary **already includes the .NET runtime**.  

The only requirement is that you have some .NET 10 file-based scripts, then compile them using the steps below — or use the included GitHub Actions workflow.  

> 💡 **Note:** The binary might be large, but you can compress it. Still, it’s better than installing .NET on every machine.  
> Fuzr is most useful if you have **multiple scripts**. If you only have one, you can just compile it the usual way with `dotnet build`.  

You can also modify `CMakeLists.txt` to change the assembly name, version, and other details to suit your program or company.

---

![License](https://img.shields.io/badge/license-MIT--0-green)
![.NET](https://img.shields.io/badge/.NET-10+-blue)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-lightgrey)

---

## 📋 Requirements

- **Git**
- **CMake** (multi-platform and easy to use)
- **.NET 10** or later

---

## ⚙️ Build Instructions

1. **Clone with submodules**  
   ```bash
   git clone --recurse-submodules https://github.com/Fuzr-Project/Fuzr.git
   ```

2. **Enter the `Fuzr` folder and generate the makefiles**  
   ```bash
   cmake -S . -B build
   ```

3. **Build everything**  
   ```bash
   cmake --build build
   ```

4. If everything is okay, a `bin` folder will be created containing the **Fuzr executable**.

---

## 🚀 Quick Start Example

1. Create a simple `.cs` script (example: `hello.cs`):
   ```csharp
   using System;
   Console.WriteLine("Hello from Fuzr!");
   ```

2. Add a matching config file (`config.yaml`):
   ```yaml
   name: hello
   description: Prints Hello from Fuzr
   ```

3. Compile Fuzr (see **Build Instructions**) and run:
   ```bash
   ./FuzrExecutable hello
   ```

---

## 🛠 Configurations

For production builds, it’s recommended to edit **`CMakeLists.txt`** and update:  
- `FUZR_ASSEMBLY_NAME`  
- `SCRIPTS_VERSION`

---

## 📄 Script Config File

Each script needs a **config file**.  
You can find an example in `Scripts/Example/config.yaml`.

Important notes:
- The `name` field is used to call your script inside the Fuzr executable.
- The `name` should match your script filename (without the `.cs` extension).  
  For example:  
  If your script is `export-db.cs`, set:
  ```yaml
  name: export-db
  ```

Other fields are just for showing information in the help page.

---

## 🤖 GitHub Actions

Fuzr comes with a **GitHub Actions workflow** for automated builds.  
Check the [`.github/workflows`](.github/workflows) folder for examples.

---

## ⚠️ Known Limitations

- The compiled binary is large, but can be compressed to reduce size.
- Designed for .NET 10+ file-based apps only.
- Best used for multiple scripts rather than a single one.
- Runtime is included in the binary — which increases size but improves portability.

---

## 🤝 Contributing

Contributions are welcome!  
Feel free to open issues or submit pull requests to improve Fuzr.  
Just make sure your code works on **Windows, Linux, and macOS**.

---

## 📜 License

[MIT-0](LICENSE)
