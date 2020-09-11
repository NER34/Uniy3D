using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CellGenerator : MonoBehaviour
{
    [SerializeField] CellSpawner spawner;

    private bool[,,] cells;
    private List<Vector3Int> changedCells;

    private Vector3Int[] glider =
    {
        new Vector3Int(3, 3, 3),
        new Vector3Int(3, 4, 4),
        new Vector3Int(3, 4, 5),
        new Vector3Int(4, 3, 3)
    };

    private Vector3Int[] oscillator =
    {
        new Vector3Int(3, 2, 3),
        new Vector3Int(3, 2, 4),
        new Vector3Int(3, 2, 5),
        new Vector3Int(3, 3, 3),
        new Vector3Int(3, 3, 5)
    };

    private Vector3Int[] invariant =
    {
        new Vector3Int(1, 2, 2),
        new Vector3Int(1, 3, 2),
        new Vector3Int(2, 2, 2),
        new Vector3Int(2, 3, 2)
    };

    private void Start()
    {
        changedCells = new List<Vector3Int>();
        cells = new bool[Parameters.length, Parameters.length, Parameters.length];

        for (int x = 0; x < Parameters.length; x++)
            for (int y = 0; y < Parameters.length; y++)
                for (int z = 0; z < Parameters.length; z++)
                    cells[x, y, z] = false;

        switch (Parameters.mode)
        {
            case "Custom":
                GenerateCustomParameters();
                break;
            case "Glider":
                GenerateFigures(glider);
                break;
            case "Invariant":
                GenerateFigures(invariant);
                break;
            case "Oscillator":
                GenerateFigures(oscillator);
                break;
        }
    }

    private void GenerateFigures(Vector3Int[] figure)
    {
        Vector3Int vector = new Vector3Int();
        for (int i = 0; i < Parameters.count; i++)
        {
            vector.x = UnityEngine.Random.Range(0, Parameters.length);
            vector.y = UnityEngine.Random.Range(0, Parameters.length);
            vector.z = UnityEngine.Random.Range(0, Parameters.length);

            for (int j = 0; j < figure.Length; j++)
            {
                Vector3Int cell = figure[j] + vector;
                cell.x %= Parameters.length;
                cell.y %= Parameters.length;
                cell.z %= Parameters.length;
                changedCells.Add(cell);
                cells[cell.x, cell.y, cell.z] = true;
            }
        }
    }

    private void GenerateCustomParameters()
    {

        for (int i = 0; i < Parameters.count; i++)
        {
            bool found = false;
            while (!found)
            {
                int x = UnityEngine.Random.Range(0, Parameters.length);
                int y = UnityEngine.Random.Range(0, Parameters.length);
                int z = UnityEngine.Random.Range(0, Parameters.length);

                if (!cells[x, y, z])
                {
                    cells[x, y, z] = true;
                    changedCells.Add(new Vector3Int(x, y, z));
                    found = true;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        spawner.UpdateCells(changedCells);

        NextGeneration();
    }

    private void NextGeneration()
    {
        changedCells.Clear();
        bool[,,] tmp_cells = new bool[Parameters.length, Parameters.length, Parameters.length];
        for (int x = 0; x < Parameters.length; x++)
            for (int y = 0; y < Parameters.length; y++)
                for (int z = 0; z < Parameters.length; z++)
                {
                    tmp_cells[x, y, z] = cells[x, y, z];

                    int neigh_count = 0;
                    for (int m_x = -1; m_x < 2; m_x++)
                        for (int m_y = -1; m_y < 2; m_y++)
                            for (int m_z = -1; m_z < 2; m_z++)
                                if (m_x != 0 || m_y != 0 || m_z != 0)
                                    if (cells[(x + m_x + Parameters.length) % Parameters.length, (y + m_y + Parameters.length) % Parameters.length, (z + m_z + Parameters.length) % Parameters.length])
                                        neigh_count++;

                    if (tmp_cells[x, y, z])
                    {
                        if (!Parameters.neighborsToLive.Contains(neigh_count))
                        {
                            tmp_cells[x, y, z] = false;
                            changedCells.Add(new Vector3Int(x, y, z));
                        }
                    }
                    else
                    {
                        if (Parameters.neighborsToBorn.Contains(neigh_count))
                        {
                            tmp_cells[x, y, z] = true;
                            changedCells.Add(new Vector3Int(x, y, z));
                        }
                    }
                }
        cells = tmp_cells;
    }
}
