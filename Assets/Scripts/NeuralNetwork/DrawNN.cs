using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NeuralNet.NeuralNetworkUtils;

namespace NeuralNet
{
    public class DrawNN : MonoBehaviour
    {

        private NeuralNetwork nn;

#pragma warning disable 0649
        [SerializeField]
        Camera nncamera;

        [SerializeField]
        Material lrmat;

#pragma warning restore 0649

        private static float half_lenght = 8;
        private static float half_height = 4.5f;
        private static float neuron_length = 0.5f;
        private static int num_inputs;

        List<List<int>> neuron_innovs;
        List<GameObject> cubes;
        List<Tuple<int, int, int>> index_ni_cubes;
        SortedDictionary<int, GameObject> neuron_and_cube;


        float space_between_neurons_hor;
        float space_between_neurons_ver;

        float position_ver;
        float position_hor;



    public void DrawNeuralNetwork(NeuralNetwork _nn)
    {

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("drawnn"))
        {
            Destroy(g);
        }

        neuron_innovs = new List<List<int>>();
        cubes = new List<GameObject>();
        neuron_and_cube = new SortedDictionary<int, GameObject>();

        position_ver = half_height;
        position_hor = -half_lenght;

        index_ni_cubes = new List<Tuple<int, int, int>>();





        this.nn = _nn;





        num_inputs = nn.Inputs().Count;


        DrawNeurons();

        DrawConnections();

    }




    private void DrawNeurons()
    {
        foreach (int ni in nn.Inputs())
        {
            DrawLayer(nn.FindNeuron(ni), 0);
        }

        space_between_neurons_hor = (2 * half_lenght - neuron_length) / (neuron_innovs.Count - 1);

        int max = 0;
        foreach (List<int> l in neuron_innovs)
        {
            if (l.Count > max) max = l.Count;
        }

        space_between_neurons_ver = (2 * half_height - neuron_length) / (max - 1);

        foreach (Tuple<int, int, int> c in index_ni_cubes)
        {
            cubes[c.Item3].transform.localPosition = new Vector3(position_hor + space_between_neurons_hor * c.Item1, position_ver - space_between_neurons_ver * c.Item2, 1);
        }


    }

    private void DrawLayer(Neuron n, int nlayer)
    {


        bool notin = true;
        foreach (List<int> l in neuron_innovs)
        {
            if (l.Contains(n.NeuronInnov())) notin = false;
        }
        if (notin)
        {
            while (neuron_innovs == null || neuron_innovs.Count < nlayer + 1)
            {
                neuron_innovs.Add(new List<int>());
            }

            cubes.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
            cubes[cubes.Count - 1].tag = "drawnn";
            neuron_and_cube[n.NeuronInnov()] = cubes[cubes.Count - 1];

            neuron_innovs[nlayer].Add(n.NeuronInnov());
            index_ni_cubes.Add(new Tuple<int, int, int>(nlayer, neuron_innovs[nlayer].Count - 1, cubes.Count - 1));

            cubes[cubes.Count - 1].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            cubes[cubes.Count - 1].transform.SetParent(nncamera.transform);


            foreach (int c in n.GetConnectionsForward())
            {
                if (nn.FindConnection(c).IsEnabled()) DrawLayer(nn.FindNeuron(nn.FindConnection(c).OutputNeuronInnov()), nlayer + 1);
            }
        }
    }




    private void DrawConnections()
    {
        Neuron neu;
        foreach (int n in nn.Inputs())
        {
            neu = nn.FindNeuron(n);
            DrawLocalConnections(neu);
        }
    }

    private void DrawLocalConnections(Neuron neu)
    {
        foreach (int c in neu.GetConnectionsForward())
        {
            if (neu.GetActualRole() != Role.output)
            {
                LineRenderer lr;
                if (neuron_and_cube[neu.NeuronInnov()].GetComponent<LineRenderer>() == null)
                {
                    lr = neuron_and_cube[neu.NeuronInnov()].AddComponent<LineRenderer>();
                    if (nn.FindNeuron(nn.FindConnection(c).OutputNeuronInnov()).GetActualRole() == Role.output)
                    {
                        lr.material = lrmat;
                    }
                    lr.startWidth = 0.1f;
                    lr.endWidth = 0.1f;
                    lr.SetPosition(0, neuron_and_cube[neu.NeuronInnov()].transform.position);
                    lr.SetPosition(1, neuron_and_cube[nn.FindConnection(c).OutputNeuronInnov()].transform.position);
                }
                else
                {
                    GameObject cubecopy = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cubecopy.tag = "drawnn";
                    cubecopy.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    cubecopy.transform.SetParent(nncamera.transform);
                    cubecopy.transform.localPosition = neuron_and_cube[neu.NeuronInnov()].transform.localPosition;
                    lr = cubecopy.AddComponent<LineRenderer>();

                    if (nn.FindNeuron(nn.FindConnection(c).OutputNeuronInnov()).GetActualRole() == Role.output)
                    {
                        lr.material = lrmat;
                    }

                    lr.startWidth = 0.1f;
                    lr.endWidth = 0.1f;
                    lr.SetPosition(0, cubecopy.transform.position);
                    lr.SetPosition(1, neuron_and_cube[nn.FindConnection(c).OutputNeuronInnov()].transform.position);


                }



                DrawLocalConnections(nn.FindNeuron(nn.FindConnection(c).OutputNeuronInnov()));
            }

        }
    }
}

}