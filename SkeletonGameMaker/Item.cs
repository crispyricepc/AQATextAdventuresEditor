using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace SkeletonGameMaker
{
    public static class Statuses
    {
        public static string[] Size = new string[4] { "tiny", "small", "medium", "large" };
        public static string[] OpenClose = new string[2] { "open", "close" };
        public static string[] Door = new string[3] { "open", "close", "locked" };
    }

    /// <summary>
    /// Contains the details and methods associated with items
    /// </summary>
    public class Item
    {
        public bool IsGettable
        {
            get
            {
                if (GetStatus().Contains("gettable"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public int ID, Location;
        public string Description, Status, Name, Commands, Results;

        public int GetDoorCounterpart(List<Item> allItems)
        {
            foreach (Item item in allItems)
            {
                if (item.ID + 10000 == ID || ID + 10000 == item.ID)
                {
                    return item.ID;
                }
            }
            return -1;
        } // returns -1 if there is no door
        public string GetDoorColour()
        {
            if (GetDoorCounterpart(Saves.Items) != -1)
            {
                string tmp = Name.TrimEnd(' ');
                tmp = tmp.Remove(tmp.LastIndexOf(' '));
                return tmp;
            }

            else return "";
        }

        public List<string> GetStatus()
        {
            string[] statusarray = Status.Split(',');
            List<string> statuslist = statusarray.ToList();
            return statuslist;
        }
        public List<string> GetCommands()
        {
            if (Commands != "")
            {
                string[] commandArray = Commands.Split(',');
                List<string> commandList = commandArray.ToList();

                if (IsGettable)
                {
                    commandList.Remove("get");
                }

                return commandList;
            }
            else return new List<string>();
        }
        public List<string[]> GetResults()
        {
            string[] mainSeg = Results.Split(';');
            List<string[]> finalArray = new List<string[]>();
            foreach (string i in mainSeg)
            {
                string[] finalSeg = i.Split(',');
                if (finalSeg.Length > 1)
                {
                    string resultParam = "";
                    for (int a = 1; a < finalSeg.Length; a++)
                    {
                        if (a == finalSeg.Length - 1 && a != 1)
                        {
                            resultParam += ",";
                        }
                        resultParam += finalSeg[a];
                    }
                    finalArray.Add(new string[2] { finalSeg[0], resultParam });
                }
            }
            return finalArray.ToList();
        }

        public void OverWriteStatus(List<string> statusList)
        {
            string newStatus = "";
            int count = 0;

            foreach (string newStrStatus in statusList)
            {
                count++;
                newStatus += newStrStatus;
                
                if (count != statusList.Count)
                {
                    newStatus += ",";
                }
            }
            
            Status = newStatus;
        }
        public void OverWriteCommands(List<string> commandList)
        {
            string newCommand = "";
            int count = 0;

            foreach (string newStrStatus in commandList)
            {
                count++;
                newCommand += newStrStatus;
                if (count != commandList.Count)
                {
                    newCommand += ",";
                }
            }

            if (IsGettable)
            {
                if (newCommand.Length != 0)
                {
                    newCommand += ",";
                }
                newCommand += "get";
            }

            Commands = newCommand;
        }
        public void OverWriteResults(List<string[]> resultList)
        {
            string newResults = "";
            int maincount = 0;

            foreach (string[] resultSet in resultList)
            {
                int setcount = 0;
                maincount++;
                foreach (string miniResult in resultSet)
                {
                    setcount++;
                    newResults += miniResult;
                    if (setcount != resultSet.Length)
                    {
                        newResults += ",";
                    }
                }

                if (maincount != resultList.Count)
                {
                    newResults += ";";
                }
            }

            Results = newResults;
        }

        public void AddStatus(string newStrStatus)
        {
            ValidateStatusAddition(newStrStatus.ToLower());
            List<string> currentStatus = GetStatus();
            List<string> commandList = GetCommands();
            currentStatus.Add(newStrStatus.ToLower());
            OverWriteStatus(currentStatus);
            OverWriteCommands(commandList);
        }
        public void RemoveStatus (string statusToRemove)
        {
            List<string> statusList = GetStatus();
            List<string> commandList = GetCommands();
            statusList.Remove(statusToRemove.ToLower());
            OverWriteStatus(statusList);
            OverWriteCommands(commandList);
        }
        public bool HasStatus(string check)
        {
            List<string> statusList = GetStatus();
            foreach (string status in statusList)
            {
                if (check == status)
                {
                    return true;
                }
            }
            return false;
        }
        public void ValidateStatusAddition(string newStatus)
        {
            if (HasStatus(newStatus))
            {
                throw new InputException("The status you are adding is already registered to this item");
            }
            
            if (Statuses.Size.Contains(newStatus) && GetStatus().Intersect(Statuses.Size).Any())
            {
                throw new InputException("The item can't have two size statuses");
            }

            if (Statuses.Door.Contains(newStatus) && GetDoorCounterpart(Saves.Items) == -1)
            {
                throw new InputException("You can't have a door status on a non-door item");
            }

            if (Statuses.OpenClose.Contains(newStatus) && GetStatus().Intersect(Statuses.OpenClose).Any())
            {
                throw new InputException("The door can only be either open or closed, not both");
            }

            if (newStatus == "gettable" && GetCommands().Contains("get"))
            {
                throw new InputException("The item contains a get command already. Delete the get command first before adding a gettable status");
            }
        }

        public void AddCommand(string newCommand, string[] commandResult)
        {
            ValidateCommandAddition(newCommand, commandResult);

            List<string> currentCommands = GetCommands();
            List<string[]> currentResults = GetResults();
            currentCommands.Add(newCommand);
            if (commandResult.Length != 0)
            {
                currentResults.Add(commandResult);
            }
            OverWriteCommands(currentCommands);
            OverWriteResults(currentResults);
        }
        public void UpdateCommandResult(string command, string[] result)
        {
            ValidateResultUpdate(result, command);

            List<string> commandList = GetCommands();
            List<string[]> resultList = GetResults();

            int index = commandList.IndexOf(command);
            resultList[index] = result;

            OverWriteResults(resultList);
        }
        public void RemoveCommand(string commandToRemove)
        {
            List<string> commandList = GetCommands();
            List<string[]> resultList = GetResults();

            int index = commandList.IndexOf(commandToRemove.ToLower());
            commandList.RemoveAt(index);
            resultList.RemoveAt(index);

            OverWriteCommands(commandList);
            OverWriteResults(resultList);
        }
        public bool IsResultNa(string command)
        {
            List<string> commandList = GetCommands();
            try
            {
                List<string[]> resultList = GetResults();
                if (IsGettable && command == "get")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public void ValidateCommandAddition(string newCommand, string[] newResult)
        {
            List<string> commands = GetCommands();
            List<string[]> results = GetResults();
            List<string> statuses = GetStatus();
            if (commands.Contains(newCommand))
            {
                throw new InputException("Two commands of the same type cannot be added (try removing the first instance before adding a new one)");
            }
            if (newCommand == "get" && !IsGettable)
            {
                throw new InputException("The item is gettable, therefore you cannot add a get command. Delete the gettable status and try again");
            }

            ValidateResultUpdate(newResult, newCommand);
        }
        public void ValidateResultUpdate(string[] newResult, string command)
        {
            foreach (string miniResult in newResult)
            {
                if ((miniResult.Contains(',') || miniResult.Contains(';')) && newResult[0].ToLower() != "roll")
                {
                    throw new InputException("A result cannot contain a semicolon or a coma, these characters are reserved by the program");
                }

                if (miniResult.Contains('"'))
                {
                    throw new InputException("A result cannot contain a speechmark, use singlequotes instead");
                }
            }
        }
    }
}
