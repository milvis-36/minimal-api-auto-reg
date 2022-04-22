# Mise

- Cílem tohoto projektu je vyzkoušet si nejlepší strukturu endpointů pomocí Minimal API.
- Sekundárním cílem je otestování konveční registrace endpointů pomocí SourceCode generátorů.


Jak udělat source code generator
- netstd 2.0
- musí najít implementaci toho IFace
- musí najít registraci 
- oboje asi přes interface

**TODO**:
- udělat konveční path, aby nebyl povinný 
- použít strategy na určení typu konvence
- konvence s ohledem na parametry
- u konvečního endpointu mít error v případě, že není metoda handler