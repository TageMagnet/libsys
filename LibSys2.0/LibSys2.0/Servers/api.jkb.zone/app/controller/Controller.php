<?php

use Medoo\Medoo;

abstract class Controller
{
    protected $conn = null;
    public function __construct()
    {
        // Initialize
        $this->conn = new Medoo([
            'charset' => 'utf8mb4',
            'collation' => 'utf8mb4_general_ci',
            'database_type' => getenv('database_type'),
            'database_name' => getenv('database_name'),
            'server' => getenv('server'),
            'username' => getenv('username'),
            'password' => getenv('password')
        ]);
    }

    /**
     * Override existing connection and settings
     *
     * @return void
     */
    protected function CreateNewConnection($options): void
    {
        $this->conn = new Medoo($options);
    }
}
