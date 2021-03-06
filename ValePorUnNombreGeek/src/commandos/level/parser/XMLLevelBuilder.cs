﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using Microsoft.DirectX;
using System.IO;
using System.Xml;
using AlumnoEjemplos.ValePorUnNombreGeek.src.commandos.terrain;
using TgcViewer.Utils.TgcSceneLoader;
using AlumnoEjemplos.ValePorUnNombreGeek.src.commandos.character;
using AlumnoEjemplos.ValePorUnNombreGeek.src.commandos.character.soldier;
using AlumnoEjemplos.ValePorUnNombreGeek.src.commandos.objects;
using AlumnoEjemplos.ValePorUnNombreGeek.src.commandos.terrain.divisibleTerrain;


namespace AlumnoEjemplos.ValePorUnNombreGeek.src.commandos.level.levelParser
{
    class XMLLevelParser
    {
        XmlElement root;
        String mediaDir;

        public XMLLevelParser(String filePath, String mediaDir)
        {
            this.root = loadXML(filePath);
            this.mediaDir = mediaDir;
         }

         private XmlElement loadXML(String filePath)
        {
            string str = File.ReadAllText(filePath);
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(str);
            XmlElement root = dom.DocumentElement;
            return root;
        }

        public Level getLevel()
        {
            ITerrain terrain = getTerrain();
            Level level = new Level(terrain);
            foreach (Enemy e in getEnemies(terrain)) level.add(e);
            foreach (Commando c in getCommandos(terrain)) level.add(c);
            foreach (ILevelObject o in getLevelObjects(terrain)) level.add(o);
            return level;
        }

        private ITerrain getTerrain()
        {
            XmlNode xmlTerrain = root.GetElementsByTagName("terrain")[0];
            return XMLTerrain.getTerrain(xmlTerrain, mediaDir);
        }


        private IEnumerable<ILevelObject> getLevelObjects(ITerrain terrain)
        {
            List<ILevelObject> levelObjects = new List<ILevelObject>();

            XmlNodeList objectNodes = root.GetElementsByTagName("levelObject");

            foreach (XmlNode node in objectNodes)
            {
                levelObjects.Add(XMLLevelObject.getLevelObject(node, terrain, mediaDir));
            }

            return levelObjects;
        }


        private IEnumerable<Commando> getCommandos(ITerrain terrain)
        {
            List<Commando> commandos = new List<Commando>();

           
            XmlNodeList commandoNodes = root.GetElementsByTagName("commando");

            foreach (XmlNode node in commandoNodes)
            {
               commandos.Add(XMLCommando.getCommando(node, terrain));
            }

            return commandos;
        }


        private IEnumerable<Enemy> getEnemies(ITerrain terrain)
        {
           
            List<Enemy> enemies = new List<Enemy>();

           
            XmlNodeList enemyNodes = root.GetElementsByTagName("enemy");
            
            foreach (XmlNode node in enemyNodes)
            {                
                enemies.Add(XMLEnemy.getEnemy(node, terrain));                         
            }

            return enemies;
        }

      
    }
}
