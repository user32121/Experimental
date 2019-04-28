using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Experimental
{
    public partial class Fondere : Form
    {
        List<Node> trie = new List<Node>();
        int maxChar;
        int rngRange;
        int prevChars;
        Random rng = new Random();

        public Fondere()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TextMaxChar_TextChanged(this, EventArgs.Empty);
            TextRngRange_TextChanged(this, EventArgs.Empty);
            TextPrevChar_TextChanged(this, EventArgs.Empty);
        }

        private void ButtonBuild_Click(object sender, EventArgs e)
        {
            //refreshes the trie
            trie.Clear();
            trie.Add(new Node('a'));

            //create node array
            string sample = textSample.Text;
            for (int i = 0; i < sample.Length; i++)
                if (!char.IsLetter(sample[i]))
                {
                    sample = sample.Remove(i, 1);
                    i--;
                }
            sample = sample.ToLower();

            //iterates through each character and adds it to the trie
            for (int i = 0; i < sample.Length - prevChars; i++)
            {
                int index = 0;
                int prevIndex = -1;
                //iterator for chain of characters
                for (int j = 0; j <= prevChars; j++)
                {
                    bool done = false;
                    //waits untill finished adding node
                    while (!done)
                    {
                        prevIndex = index;
                        if (trie[index].letter == sample[i + j])
                        {
                            //if correct node, adds 1 to uses for that node
                            trie[index].uses++;
                            done = true;
                        }
                        else
                        {
                            if (trie[prevIndex].nextNode == -1)
                            {
                                //if there is no nextNode, creates new node
                                trie.Add(new Node(sample[i + j]) { uses = 1 });
                                index = trie.Count - 1;
                                trie[prevIndex].nextNode = index;
                                done = true;
                            }
                            else
                            {
                                //if there is nextNode, moves index to nextNode
                                index = trie[prevIndex].nextNode;
                            }
                        }
                    }

                    prevIndex = index;
                    if (trie[prevIndex].daughterNode == -1)
                    {
                        //creates daughter node and moves index
                        trie.Add(new Node('a'));
                        index = trie.Count - 1;
                        trie[prevIndex].daughterNode = index;
                    }
                    else
                    {
                        //moves index to daughterNode
                        index = trie[prevIndex].daughterNode;
                    }
                }
            }
        }

        private void TextMaxChar_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textMaxChar.Text, out int result) && result > 0)
                maxChar = result;
        }
        private void TextRngRange_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textRngRange.Text, out int result) && result > 0)
                rngRange = result;
        }
        private void TextPrevChar_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textPrevChar.Text, out int result) && result > 0)
                prevChars = result;
        }

        private void ButtonGenerate_Click(object sender, EventArgs e)
        {
            textOutput.Text = "";

            foreach (char start in new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
                'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' })
            {
                //generates random anti words
                int index = 0;
                string word = start.ToString();

                while (word.Length < maxChar && index != -1)
                {
                    //prevCharCalculations
                    for (int i = Math.Max(0, word.Length - prevChars); i < word.Length; i++)
                    {
                        //finds word in letter path
                        while (!(trie[index].letter == word[i]))
                            if (trie[index].nextNode != -1)
                                index = trie[index].nextNode;
                            else
                                index = 0;

                        index = trie[index].daughterNode;
                    }

                    //top letter occurences
                    int[] topIndexes = new int[rngRange];

                    for (int i = 0; i < rngRange; i++)
                        topIndexes[i] = -1;

                    //finds most used letters
                    while (index != -1)
                    {
                        for (int i = 0; i < topIndexes.Length; i++)
                            if (trie[index].uses > (topIndexes[i] == -1 ? -1 : trie[topIndexes[i]].uses))
                            {
                                topIndexes[i] = index;
                                break;
                            }

                        index = trie[index].nextNode;
                    }
                    //counts the total number of valid values in topIndexes
                    int totalPass = 0;
                    for (int i = 0; i < topIndexes.Length; i++)
                        if (topIndexes[i] != -1)
                            totalPass++;

                    word += trie[topIndexes[rng.Next(totalPass)]].letter;
                    index = 0;

                }

                //outputs anti words
                textOutput.Text += word + "\r\n";
            }
        }
    }

    class Node
    {
        public char letter;
        public int nextNode = -1;
        public int daughterNode = -1;
        public int uses;

        public Node(char id)
        {
            letter = id;
        }
    }
}
