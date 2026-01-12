using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform treasure;
    [SerializeField] float energy;

    float[] inputs;
    float[] outputs;

    float[] inputLayer;
    float[] hiddenLayer;
    float[] outputLayer;

    float[,] inputToHiddenWeights;
    float[,] hiddenToOutputWeights;

    float run;
    float goToTreasure;
    float rest;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        inputs[0] = DistanceToPlayer();
        inputs[1] = DistanceToTreasure();
        inputs[2] = 1/(energy+1);

        outputs = CalculateOutputs(inputs);

        run = outputs[0];
        goToTreasure = outputs[1];
        rest = outputs[2];

        float mostImportantOutput = outputs.Max();
       // Debug.Log("run" + run);
       // Debug.Log("treasure " + goToTreasure);
       // Debug.Log("rest " + rest);


        if (mostImportantOutput < 0.1)
        {
            Rest();
        }
        else if(mostImportantOutput == run)
        {
            Run();
        }
        else if(mostImportantOutput == goToTreasure)
        {
            GoToTreasure();
        }
        else if(mostImportantOutput == rest)
        {
            Rest();
        }
    }

    public void Init()
    {
        inputs = new float[3];
        outputs = new float[3];

        inputLayer = new float[3];
        hiddenLayer = new float[4];
        outputLayer = new float[3];

        inputToHiddenWeights = new float[inputLayer.Length, hiddenLayer.Length];
        hiddenToOutputWeights = new float[hiddenLayer.Length, outputLayer.Length];

        System.Random rand = new System.Random();
        for (int i = 0; i < inputLayer.Length; i++)
            for (int j = 0; j < hiddenLayer.Length; j++)
                inputToHiddenWeights[i, j] = ((float)rand.NextDouble() * 2) - 1;

        for (int i = 0; i < hiddenLayer.Length; i++)
            for (int j = 0; j < outputLayer.Length; j++)
                hiddenToOutputWeights[i, j] = ((float)rand.NextDouble() * 2) - 1;
    }

    public float[] CalculateOutputs(float[] inputs)
    {
        if(inputs.Length != 3)
        {
            throw new ArgumentException("either too few or too many inputs");
        }

        Array.Copy(inputs, inputLayer, 3);

        //send data from inputs to hidden layer (forward propagation)
        for (int i = 0; i < hiddenLayer.Length; i++)
        {
            float sum = 0;
            for (int j = 0; j < inputLayer.Length; j++)
            {
                sum += inputLayer[j] * inputToHiddenWeights[j, i];
            }
            hiddenLayer[i] = Sigmoid(sum);
        }

        //send data from hidden to output layer (still propagating forward ig)
        for (int i = 0; i < outputs.Length; i++)
        {
            float sum = 0;
            for (int j = 0; j < hiddenLayer.Length; j++)
            {
                sum += hiddenLayer[j] * hiddenToOutputWeights[j, i];
            }
            outputLayer[i] = Sigmoid(sum);
        }

        return outputLayer;
    }

    public float Sigmoid(float input)
    {
        return (float)(1 / (1 + Math.Exp(-input)));
    }

    public float DistanceToPlayer()
    {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.position.x, player.position.z));
    }

    public float DistanceToTreasure()
    {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(treasure.position.x, treasure.position.z));
    }

    public void Rest()
    {
        Debug.Log("rest");
    }

    public void Run()
    {
        Debug.Log("run");
    }

    public void GoToTreasure()
    {
        Debug.Log("go to treasure");
    }
}
