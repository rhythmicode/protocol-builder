<?php
namespace SampleProject\Protocol\Interface;

interface Crud {
    function Create(string $param1, int $param2);
    function Query(string $param3): string;

}
