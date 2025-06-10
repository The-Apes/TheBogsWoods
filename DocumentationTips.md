# Documentation Tips

This file will cover some neat tips in documentation to help the team understand code better :)

## XML Comments

XML comments go **above classes, methods, properties, or fields** and start with `///`. They use special tags to describe what the code does, what parameters it takes, and what it returns.

Example:

```csharp
/// <summary>
/// Calculates the player's health after taking damage.
/// </summary>
/// <param name="damage">The amount of damage taken.</param>
/// <returns>The updated health value.</returns>
public int CalculateHealth(int damage) 
{
    // method code
}