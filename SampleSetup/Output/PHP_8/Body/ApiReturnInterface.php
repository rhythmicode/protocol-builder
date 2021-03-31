<?php
namespace SampleProject\Protocol\Body;

interface ApiReturnInterface {
    function Payload():         \SampleProject\Protocol\Body\AbstractReturn;
    function Meta(): string;
    /**
     * @param int[] $p2
     * @param float[] $p3
     */
    function Fill(?string $p1, $p2, $p3, ?int $p4): string;
}
