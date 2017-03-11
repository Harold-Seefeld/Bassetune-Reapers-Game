﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonGeneration.Generator.Domain;
using DungeonGeneration.Generator.Pickers;
using DungeonGeneration.Logging;

public class CaveGenBehaviour : MonoBehaviour {

    public MeshFilter _ceilMeshFilter;
    public MeshFilter _wallMeshFilter;
    public MeshFilter _floorMeshFilter;

    [Range(50, 300)]
    public int _mapMaxWidth = 200;
    [Range(50, 300)]
    public int _mapMaxHeight = 200;
    [Range(50, 100)]
    public int _cellularFillChance = 58;
    [Range(0, 10)]
    public int _cellularSmoothSteps = 5;

    [Range(0, 20)]
    public int _roomsNumberMin = 5;
    [Range(0, 20)]
    public int _roomsNumberMax = 9;

    [Range(20, 100)]
    public int _roomSizeMin = 40;
    [Range(20, 100)]
    public int _roomSizeMax = 60;

    [Range(1, 20)]
    public int _corridorLengthMin = 10;
    [Range(1, 20)]
    public int _corridorLengthMax = 15;
    [Range(1, 20)]
    public int _corridorWidthMin = 5;
    [Range(1, 20)]
    public int _corridorWidthMax = 9;

    [Range(1, 10)]
    public int _wallHeight = 5;
    [Range(0, 5)]
    public int _ceilHeightVariation = 0;
    
    public int _seed = 48;
    public bool _randomSeed = false;

    //public bool _debugShowAsGizmos = false;
    //public bool _debugShowRoomPerimeter = false;

    private CaveGridGenerator _generator;
    private CaveRenderer _renderer;

    void Awake() {
        _generator = new CaveGridGenerator();
        _renderer = CaveRenderer.newInstance(this);
    }

    void Start () {
        generate();
    }
	
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
            generate();
        }
	}

    private void generate() {  
        _generator.setMapSize(_mapMaxHeight, _mapMaxWidth);
        _generator.setRoomsNumberRange(_roomsNumberMin, _roomsNumberMax);
        _generator.setRoomSizeRange(_roomSizeMin, _roomSizeMax);
        _generator.setCorridorLengthRange(_corridorLengthMin, _corridorLengthMax);
        _generator.setCorridorWidthRange(_corridorWidthMin, _corridorWidthMax);
        //_generator.setLogger(new UnityEngineLogger());
        if (_randomSeed) {
            _seed = Time.time.ToString().GetHashCode();
        }
        _generator.setSeed(_seed);

        _generator.setCellularFillChance(_cellularFillChance);
        _generator.setCellularSmoothingSteps(_cellularSmoothSteps);

        //if(_debugShowAsGizmos) return;
        if (_generator.asBoard().isEmpty()) {
            _ceilMeshFilter.mesh = null;
            _wallMeshFilter.mesh = null;
            _wallMeshFilter.gameObject.GetComponent<MeshCollider>().sharedMesh = null;
            _floorMeshFilter.mesh = null;
            _floorMeshFilter.gameObject.GetComponent<MeshCollider>().sharedMesh = null;
            return;
        }
        _renderer.convertToMeshes(_generator.asOIGrid());
    }

   /*
    void OnDrawGizmos() {
        if (!_debugShowAsGizmos) return;
        if (_shapes.Count == 0) return;
        
        foreach(IXShape each in _shapes) {
           each.accept(new BlackAndWhiteGizmoDrawer(_debugShowRoomPerimeter));
        }
    }
    */
}