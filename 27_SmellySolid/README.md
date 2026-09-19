# SOLID Violations Kata

## Overview

This is a **verification fixture, not a practice exercise**. Each folder
contains a small, self-contained example of exactly one SOLID principle
violation, translated directly from *Agile Technical Practices Distilled*'s
SOLID chapter worked examples (Car/`Save`, `CarEngineStatusReportController`,
`Chef`/`Oven`/`Microwave`, `IAmACar`, `Kitchen`/`MicrowaveOven`) -- this C#
kata is close to the book's own original C# examples. Its purpose is to
give static-analysis/AI code-review tooling (specifically
[jev-review](https://github.com/pedromsantos/jev-review)) a known-answer set
to check its SOLID rules against -- every file's violation is deliberate and
documented below, not hidden.

Equivalent kata exist for Go, Java, Python, and C too.

## What's here

| File | Violates | Why |
|---|---|---|
| `Srp/Car.cs` | SRP | `Save()` mixes a persistence concern into a class otherwise about domain behaviour (mileage/travel) |
| `Ocp/CarEngineStatusReportController.cs` | OCP (and DIP) | every new report format needs a new method here, and it constructs its concrete views directly instead of receiving them injected |
| `Lsp/Microwave.cs` | LSP | overrides `Cook()` to throw instead of honouring the base contract |
| `Lsp/Chef.cs` | -- | not itself a violation, but its `is Microwave` special-case is the client-code tell of `Microwave`'s LSP violation |
| `Isp/IAmACar.cs` | ISP | bundles `RefillGasoline`/`RefillElectricity`, capabilities no single car supports both of |
| `Isp/ElectricCar.cs` | -- | the forced implementer: throws on the gasoline method it can't honestly support |
| `Dip/Kitchen.cs` | DIP (and OCP) | constructs `MicrowaveOven` directly; can't work with any other oven without being edited |
| `Dip/MicrowaveOven.cs` | DIP | constructs `MicrowaveGenerator` directly instead of receiving it injected |
