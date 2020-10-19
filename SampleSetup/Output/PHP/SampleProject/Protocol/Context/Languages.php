<?php
namespace SampleProject\Protocol\Context;

abstract class Languages {
    const English = "en";
        
    /**
     * @deprecated Message: Please use Finnish
     */
    const Suomi = "fi-old";
        
    const Finnish = "fi";

    const MapToName = array(
        "en" => "English",
        "fi-old" => "Suomi",
        "fi" => "Finnish"
    );
}
